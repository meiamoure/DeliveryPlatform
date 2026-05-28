import { useRoutePlanning } from '../../features/route-planning/useRoutePlanning';
import { DeliverySelectionTable } from '../../features/route-planning/DeliverySelectionTable';
import { VehicleSelection } from '../../features/route-planning/VehicleSelection';
import { RoutePlanningSummary } from '../../features/route-planning/RoutePlanningSummary';
import styles from './RoutePlanningPage.module.scss';

export const RoutePlanningPage = () => {
  const {
    deliveries,
    selectedDeliveryIds,
    selectedVehicles,
    totalWeight,
    totalVolume,
    mode,
    setMode,
    date,
    setDate,
    depotNodeId,
    depotName,
    isLoading,
    isBuilding,
    error,
    buildResultMessage,
    toggleDelivery,
    toggleVehicle,
    selectedVehicleIds,
    vehicles,
    setSingleVehicleId,
    buildRoutes,
    singleVehicleCapacityWarning,
  } = useRoutePlanning();

  if (isLoading) {
    return (
      <div className={styles.page}>
        <div className={styles.message}>Завантаження...</div>
      </div>
    );
  }

  return (
    <div className={styles.page}>
      <div className={styles.topBar}>
        <div>
          <h1 className={styles.title}>Розрахувати маршрути</h1>
          <p className={styles.subtitle}>
            Вибір заявок, транспорту і побудова одного або кількох маршрутів
          </p>
        </div>
      </div>

      <div className={styles.filtersCard}>
        <div className={styles.filtersGrid}>
          <div className={styles.field}>
            <label className={styles.label}>Дата</label>
            <input
              className={styles.input}
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </div>

          <div className={styles.field}>
            <label className={styles.label}>Депо</label>
            <input
              className={styles.input}
              type="text"
              value={depotName || depotNodeId}
              disabled
            />
          </div>
        </div>

        <div className={styles.modeRow}>
          <label className={styles.radioLabel}>
            <input
              type="radio"
              checked={mode === 'single'}
              onChange={() => setMode('single')}
            />
            Один маршрут
          </label>

          <label className={styles.radioLabel}>
            <input
              type="radio"
              checked={mode === 'many'}
              onChange={() => setMode('many')}
            />
            Кілька маршрутів
          </label>
        </div>
      </div>

      <div className={styles.block}>
        <h2 className={styles.blockTitle}>Заявки</h2>
        <DeliverySelectionTable
          deliveries={deliveries}
          selectedDeliveryIds={selectedDeliveryIds}
          onToggle={toggleDelivery}
        />
      </div>

      <div className={styles.block}>
        <h2 className={styles.blockTitle}>Транспорт</h2>
        <VehicleSelection
          mode={mode}
          vehicles={vehicles}
          selectedVehicleIds={selectedVehicleIds}
          onSelectSingle={setSingleVehicleId}
          onToggleMany={toggleVehicle}
        />
      </div>

      <div className={styles.block}>
        <h2 className={styles.blockTitle}>Зведення</h2>
        <RoutePlanningSummary
          selectedDeliveriesCount={selectedDeliveryIds.length}
          totalWeight={totalWeight}
          totalVolume={totalVolume}
          mode={mode}
          selectedVehicles={selectedVehicles}
          singleVehicleCapacityWarning={singleVehicleCapacityWarning}
        />
      </div>

      {error && <div className={styles.error}>{error}</div>}
      {buildResultMessage && <div className={styles.success}>{buildResultMessage}</div>}

      <div className={styles.actions}>
        <button
          type="button"
          onClick={() => void buildRoutes()}
          disabled={isBuilding}
          className={styles.primaryButton}
        >
          {isBuilding ? 'Побудова...' : 'Розрахувати'}
        </button>
      </div>
    </div>
  );
};