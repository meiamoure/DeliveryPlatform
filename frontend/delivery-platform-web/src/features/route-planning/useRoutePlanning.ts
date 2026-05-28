import { useEffect, useMemo, useState } from 'react';
import { deliveryApi } from '../../entities/delivery/api';
import { vehicleApi } from '../../entities/vehicle/api';
import { routeApi } from '../../entities/route/api';
import type { DeliveryDto } from '../../entities/delivery/types';
import type { VehicleDto } from '../../entities/vehicle/types';

type Mode = 'single' | 'many';

export const useRoutePlanning = () => {
  const [deliveries, setDeliveries] = useState<DeliveryDto[]>([]);
  const [vehicles, setVehicles] = useState<VehicleDto[]>([]);
  const [selectedDeliveryIds, setSelectedDeliveryIds] = useState<string[]>([]);
  const [selectedVehicleIds, setSelectedVehicleIds] = useState<string[]>([]);
  const [mode, setMode] = useState<Mode>('single');
  const [date, setDate] = useState<string>(new Date().toISOString().slice(0, 10));
  const [depotNodeId, setDepotNodeId] = useState<string>('');
  const [isLoading, setIsLoading] = useState(false);
  const [isBuilding, setIsBuilding] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [buildResultMessage, setBuildResultMessage] = useState<string | null>(null);

  useEffect(() => {
    const load = async () => {
      try {
        setIsLoading(true);
        setError(null);

        const [pendingDeliveries, availableVehicles] = await Promise.all([
          deliveryApi.getPending(),
          vehicleApi.getAvailable(),
        ]);

        setDeliveries(pendingDeliveries);
        setVehicles(availableVehicles);

        const uniqueDepotIds = Array.from(
          new Set(availableVehicles.map((vehicle) => vehicle.depotNodeId))
        );

        if (uniqueDepotIds.length > 0) {
          setDepotNodeId(uniqueDepotIds[0]);
        }
      } catch (e) {
        setError('Неможливо завантажити дані для побудови маршрутів');
      } finally {
        setIsLoading(false);
      }
    };

    void load();
  }, []);

  const selectedDeliveries = useMemo(
    () => deliveries.filter((delivery) => selectedDeliveryIds.includes(delivery.id)),
    [deliveries, selectedDeliveryIds]
  );

  const selectedVehicles = useMemo(
    () => vehicles.filter((vehicle) => selectedVehicleIds.includes(vehicle.id)),
    [vehicles, selectedVehicleIds]
  );

  const totalWeight = useMemo(
    () => selectedDeliveries.reduce((sum, item) => sum + item.weightKg, 0),
    [selectedDeliveries]
  );

  const totalVolume = useMemo(
    () => selectedDeliveries.reduce((sum, item) => sum + item.volumeM3, 0),
    [selectedDeliveries]
  );

  const depotName =
  vehicles.find((vehicle) => vehicle.depotNodeId === depotNodeId)?.depotName ?? '';

  const singleVehicle = mode === 'single' ? selectedVehicles[0] : null;

  const singleVehicleCapacityWarning = useMemo(() => {
    if (!singleVehicle) return null;

    if (totalWeight > singleVehicle.maxWeightKg) {
      return `Перевищено ліміт за вагою: ${totalWeight} кг / ${singleVehicle.maxWeightKg} кг`;
    }

    if (totalVolume > singleVehicle.maxVolumeM3) {
      return `Перевищено ліміт за об'ємом: ${totalVolume} м³ / ${singleVehicle.maxVolumeM3} м³`;
    }

    return null;
  }, [singleVehicle, totalWeight, totalVolume]);

  const toggleDelivery = (deliveryId: string) => {
    setSelectedDeliveryIds((prev) =>
      prev.includes(deliveryId)
        ? prev.filter((id) => id !== deliveryId)
        : [...prev, deliveryId]
    );
  };

  const toggleVehicle = (vehicleId: string) => {
    if (mode === 'single') {
      setSelectedVehicleIds([vehicleId]);
      return;
    }

    setSelectedVehicleIds((prev) =>
      prev.includes(vehicleId)
        ? prev.filter((id) => id !== vehicleId)
        : [...prev, vehicleId]
    );
  };

  const setSingleVehicleId = (vehicleId: string) => {
    setSelectedVehicleIds(vehicleId ? [vehicleId] : []);
  };

  const buildRoutes = async () => {
    try {
      setError(null);
      setBuildResultMessage(null);

      if (!depotNodeId) {
        setError('Обрати depot');
        return;
      }

      if (selectedDeliveryIds.length === 0) {
        setError('Обрати хоча б одну заявку');
        return;
      }

      if (selectedVehicleIds.length === 0) {
        setError('Обрати хоча б один транспорт');
        return;
      }

      if (mode === 'single' && singleVehicleCapacityWarning) {
        setError(singleVehicleCapacityWarning);
        return;
      }

      setIsBuilding(true);

      if (mode === 'single') {
        const result = await routeApi.buildSingle({
          depotNodeId,
          vehicleId: selectedVehicleIds[0],
          deliveryIds: selectedDeliveryIds,
          date: new Date(date).toISOString(),
        });

        setBuildResultMessage(`Маршрут успішно збудовано. ID: ${result.id}`);
      } else {
        const result = await routeApi.buildMany({
          depotNodeId,
          vehicleIds: selectedVehicleIds,
          deliveryIds: selectedDeliveryIds,
          date: new Date(date).toISOString(),
        });

        setBuildResultMessage(
          `Побудовано маршрутів: ${result.routes.length}. Не розподілено заявок: ${result.unassignedDeliveryIds.length}`
        );
      }
    } catch (e: any) {
      setError(e?.response?.data?.detail ?? 'Не вдалося збудувати маршрут');
    } finally {
      setIsBuilding(false);
    }
  };

  return {
    deliveries,
    vehicles,
    selectedDeliveryIds,
    selectedVehicleIds,
    selectedDeliveries,
    selectedVehicles,
    totalWeight,
    totalVolume,
    mode,
    setMode,
    date,
    setDate,
    depotNodeId,
    depotName,
    setDepotNodeId,
    isLoading,
    isBuilding,
    error,
    buildResultMessage,
    toggleDelivery,
    toggleVehicle,
    setSingleVehicleId,
    buildRoutes,
    singleVehicleCapacityWarning,
  };
};
