import type { VehicleDto } from '../../entities/vehicle/types';
import styles from './VehicleSelection.module.scss';

type Props = {
  mode: 'single' | 'many';
  vehicles: VehicleDto[];
  selectedVehicleIds: string[];
  onSelectSingle: (vehicleId: string) => void;
  onToggleMany: (vehicleId: string) => void;
};

export const VehicleSelection = ({
  mode,
  vehicles,
  selectedVehicleIds,
  onSelectSingle,
  onToggleMany,
}: Props) => {
  if (vehicles.length === 0) {
    return <div className={styles.empty}>Немає доступного транспорту</div>;
  }

  if (mode === 'single') {
    return (
      <div className={styles.wrapper}>
        <select
          className={styles.select}
          value={selectedVehicleIds[0] ?? ''}
          onChange={(e) => onSelectSingle(e.target.value)}
        >
          <option value="">Обрати транспорт</option>
          {vehicles.map((vehicle) => (
            <option key={vehicle.id} value={vehicle.id}>
              {vehicle.plate} — {vehicle.maxWeightKg} кг / {vehicle.maxVolumeM3} м³
            </option>
          ))}
        </select>
      </div>
    );
  }

  return (
    <div className={styles.wrapper}>
      <div className={styles.vehicleList}>
        {vehicles.map((vehicle) => {
          const isChecked = selectedVehicleIds.includes(vehicle.id);

          return (
            <label key={vehicle.id} className={styles.vehicleCard}>
              <input
                className={styles.checkbox}
                type="checkbox"
                checked={isChecked}
                onChange={() => onToggleMany(vehicle.id)}
              />

              <div className={styles.vehicleInfo}>
                <span className={styles.plate}>{vehicle.plate}</span>
                <span className={styles.meta}>
                  Грузоподъёмность: {vehicle.maxWeightKg} кг
                </span>
                <span className={styles.meta}>
                  Объём: {vehicle.maxVolumeM3} м³
                </span>
              </div>
            </label>
          );
        })}
      </div>
    </div>
  );
};