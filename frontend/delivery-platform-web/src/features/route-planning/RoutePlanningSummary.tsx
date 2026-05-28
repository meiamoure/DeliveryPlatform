import type { VehicleDto } from '../../entities/vehicle/types';
import styles from './RoutePlanningSummary.module.scss';

type Props = {
  selectedDeliveriesCount: number;
  totalWeight: number;
  totalVolume: number;
  mode: 'single' | 'many';
  selectedVehicles: VehicleDto[];
  singleVehicleCapacityWarning: string | null;
};

export const RoutePlanningSummary = ({
  selectedDeliveriesCount,
  totalWeight,
  totalVolume,
  mode,
  selectedVehicles,
  singleVehicleCapacityWarning,
}: Props) => {

  const formatVolume = (v: number) =>
    Number(v.toFixed(2)).toString();
  return (
    <div className={styles.wrapper}>
      <div className={styles.card}>
        <div className={styles.row}>
          <span className={styles.label}>Обрано заявок</span>
          <span className={styles.value}>{selectedDeliveriesCount}</span>
        </div>

        <div className={styles.row}>
          <span className={styles.label}>Загальна вага</span>
          <span className={styles.value}>{totalWeight} кг</span>
        </div>

        <div className={styles.row}>
            <span className={styles.label}>Загальний обʼєм</span>
            <span className={styles.value}>
              {formatVolume(totalVolume)} м³
            </span>
          </div>

        <div className={styles.row}>
          <span className={styles.label}>
            {mode === 'single' ? 'Обраний транспорт' : 'Обрано авто'}
          </span>
          <span className={styles.value}>
            {mode === 'single'
              ? selectedVehicles[0]?.plate ?? '—'
              : selectedVehicles.length}
          </span>
        </div>
      </div>

      {singleVehicleCapacityWarning ? (
        <div className={styles.warning}>{singleVehicleCapacityWarning}</div>
      ) : (
        <div className={styles.success}>Обмеження не перевищені</div>
      )}
    </div>
  );
};