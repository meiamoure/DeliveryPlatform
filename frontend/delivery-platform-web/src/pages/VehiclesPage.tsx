import { useEffect, useMemo, useState } from 'react';
import styles from './VehiclesPage.module.scss';
import { getVehicles, setVehicleStatus, unassignVehicle } from '../entities/vehicle/api';
import type { VehicleDto } from '../entities/vehicle/types';
import { vehicleStatusLabels, vehicleStatusClassMap, type VehicleStatus } from '../entities/vehicle/status';
type StatusFilter = 'All' | VehicleStatus;

export const VehiclesPage = () => {
  const [vehicles, setVehicles] = useState<VehicleDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [search, setSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState<StatusFilter>('All');

  const loadVehicles = async () => {
    try {
      setLoading(true);
      setError(null);

      const data = await getVehicles();
      setVehicles(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Помилка загрузки транспорту');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    void loadVehicles();
  }, []);

  const filteredVehicles = useMemo(() => {
    return vehicles.filter((vehicle) => {
      const matchesSearch = vehicle.plate.toLowerCase().includes(search.toLowerCase());
      const matchesStatus = statusFilter === 'All' || vehicle.status === statusFilter;

      return matchesSearch && matchesStatus;
    });
  }, [vehicles, search, statusFilter]);

  const stats = useMemo(() => {
    return {
      total: vehicles.length,
      available: vehicles.filter((v) => v.status === 'Available').length,
      busy: vehicles.filter((v) => v.status === 'Busy').length,
      maintenance: vehicles.filter((v) => v.status === 'Maintenance').length,
      disabled: vehicles.filter((v) => v.status === 'Disabled').length,
    };
  }, [vehicles]);

  const handleStatusChange = async (id: string, status: VehicleStatus) => {
    try {
      await setVehicleStatus(id, { status });
      await loadVehicles();
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Помилка зміни статусу транспорту');
    }
  };

  const handleUnassign = async (id: string) => {
    try {
      await unassignVehicle(id);
      await loadVehicles();
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Помилка зняття транспорту з маршруту');
    }
  };

  return (
    <div className={styles.page}>
      <div className={styles.topBar}>
        <div>
          <h1 className={styles.title}>Транспорт</h1>
          <p className={styles.subtitle}>Управління машинами та їх статусами</p>
        </div>

        <button className={styles.primaryButton}>
          Додати транспорт
        </button>
      </div>

      <div className={styles.stats}>
        <div className={styles.statCard}>
          <span>Всього</span>
          <strong>{stats.total}</strong>
        </div>
        <div className={styles.statCard}>
          <span>Вільно</span>
          <strong>{stats.available}</strong>
        </div>
        <div className={styles.statCard}>
          <span>Занято</span>
          <strong>{stats.busy}</strong>
        </div>
        <div className={styles.statCard}>
          <span>На обслуговуванні</span>
          <strong>{stats.maintenance}</strong>
        </div>
        <div className={styles.statCard}>
          <span>Вимкнено</span>
          <strong>{stats.disabled}</strong>
        </div>
      </div>

      <div className={styles.toolbar}>
        <input
          className={styles.searchInput}
          type="text"
          placeholder="Пошук по номеру..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <select
          className={styles.select}
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value as StatusFilter)}
        >
          <option value="All">Усі статуси</option>
          <option value="Available">Вільний</option>
          <option value="Busy">Занятий</option>
          <option value="Maintenance">На обслуговуванні</option>
          <option value="Disabled">Вимкнено</option>
        </select>
      </div>

      {loading && <div className={styles.message}>Загрузка...</div>}
      {error && <div className={styles.error}>{error}</div>}

      {!loading && !error && filteredVehicles.length === 0 && (
        <div className={styles.empty}>
          Транспорт не знайдено
        </div>
      )}

      {!loading && !error && filteredVehicles.length > 0 && (
        <div className={styles.grid}>
          {filteredVehicles.map((vehicle) => (
            <div key={vehicle.id} className={styles.card}>
              <div className={styles.cardHeader}>
                <div>
                  <div className={styles.plate}>{vehicle.plate}</div>
                  <div className={styles.depot}>
                    База: {vehicle.depotName ?? 'Не вказано'}
                  </div>
                </div>

                <span
                  className={`${styles.statusBadge} ${styles[vehicleStatusClassMap[vehicle.status]]}`}
                >
                  {vehicleStatusLabels[vehicle.status]}
                </span>
              </div>

              <div className={styles.infoGrid}>
                <div className={styles.infoItem}>
                  <span>Макс. вага</span>
                  <strong>{vehicle.maxWeightKg} кг</strong>
                </div>

                <div className={styles.infoItem}>
                  <span>Макс. об'єм</span>
                  <strong>{vehicle.maxVolumeM3} м³</strong>
                </div>

                <div className={styles.infoItem}>
                  <span>Швидкість</span>
                  <strong>{vehicle.speedKmh} км/ч</strong>
                </div>

                <div className={styles.infoItem}>
                  <span>Маршрут</span>
                  <strong>{vehicle.currentRouteId ? 'Назначений' : 'Немає'}</strong>
                </div>
              </div>

              <div className={styles.actions}>
                {vehicle.currentRouteId && (
                  <button
                    className={styles.secondaryButton}
                    onClick={() => void handleUnassign(vehicle.id)}
                  >
                    Снять с маршрута
                  </button>
                )}

                <button
                  className={styles.secondaryButton}
                  onClick={() => void handleStatusChange(vehicle.id, 'Available')}
                >
                  Вільний
                </button>

                <button
                  className={styles.secondaryButton}
                  onClick={() => void handleStatusChange(vehicle.id, 'Busy')}
                >
                  Занятий
                </button>

                <button
                  className={styles.secondaryButton}
                  onClick={() => void handleStatusChange(vehicle.id, 'Maintenance')}
                >
                  На обслуговуванні
                </button>

                <button
                  className={styles.dangerButton}
                  onClick={() => void handleStatusChange(vehicle.id, 'Disabled')}
                >
                  Вимкнути
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};