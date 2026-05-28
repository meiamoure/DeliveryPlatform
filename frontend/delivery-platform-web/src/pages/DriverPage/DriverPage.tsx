import { useDriverRoute } from '../../features/driver/useDriverRoute';
import { RouteMap } from '../../widgets/RouteMap';
import styles from './DriverPage.module.scss';

const statusMap: Record<string, string> = {
  Planned: 'Заплановано',
  Accepted: 'Прийнято',
  InProgress: 'Виконується',
  Completed: 'Завершено',
};

export const DriverPage = () => {
  const { route, loading, accept, start, complete } = useDriverRoute();

  if (loading) {
    return <div className={styles.page}>Завантаження...</div>;
  }

  if (!route) {
    return <div className={styles.page}>Немає активного маршруту</div>;
  }

  const sortedStops = [...route.stops].sort(
    (a, b) => a.sequence - b.sequence
  );

  const nextStop = sortedStops[1];

  const renderButtons = () => {
    switch (route.status) {
      case 'Planned':
        return <button onClick={accept}>Прийняти маршрут</button>;

      case 'Accepted':
        return <button onClick={start}>Почати маршрут</button>;

      case 'InProgress':
        return <button onClick={complete}>Завершити маршрут</button>;

      case 'Completed':
        return <span className={styles.completed}>Маршрут завершено</span>;
    }
  };

  const mapRoute = route
  ? {
      id: route.id,
      vehicleId: '', 
      vehiclePlate: route.vehiclePlate,
      driverId: null,
      serviceDate: route.serviceDate,
      status: route.status,

      orderedPoints: route.orderedPoints,

      path: route.path ?? [],

      segments: [],

      stops: route.stops.map((s) => ({
        nodeId: s.nodeId,
        nodeName: s.nodeName,
        lat: s.lat,
        lng: s.lng,
        orderNumber: s.orderNumber,
        sequence: s.sequence,
      })),

      totalDistanceKm: route.totalDistanceKm,
      totalTimeMin: route.totalTimeMin,

      number: route.number,
      code: route.code,
    }
  : null;

  return (
    <div className={styles.page}>
      <h1 className={styles.title}>Мій маршрут</h1>

      <div className={styles.layout}>
        <div className={styles.map}>
          <RouteMap route={mapRoute} />
        </div>

        
        <div className={styles.sidebar}>
          <div className={styles.card}>
            <div>🚚 {route.vehiclePlate}</div>
            <div>📅 {route.serviceDate}</div>
            <div>📍 {statusMap[route.status]}</div>
          </div>

          <div className={styles.nextStop}>
            <h4>Наступна точка</h4>

            {nextStop ? (
              <>
                <div className={styles.name}>{nextStop.nodeName}</div>

                {nextStop.orderNumber && (
                  <div className={styles.order}>
                    📦 {nextStop.orderNumber}
                  </div>
                )}
              </>
            ) : (
              <div>Маршрут завершено</div>
            )}
          </div>

          <div className={styles.stops}>
            <h3>Зупинки</h3>

            {sortedStops.map((stop, index) => {
              const isNext = index === 1;

              return (
                <div
                  key={stop.sequence}
                  className={`${styles.stop} ${
                    isNext ? styles.activeStop : ''
                  }`}
                >
                  <div className={styles.index}>{index}</div>

                  <div className={styles.stopInfo}>
                    <div className={styles.name}>{stop.nodeName}</div>

                    {stop.orderNumber && (
                      <div className={styles.order}>
                        {stop.orderNumber}
                      </div>
                    )}
                  </div>
                </div>
              );
            })}
          </div>

          <div className={styles.actions}>
            {renderButtons()}
          </div>
        </div>
      </div>
    </div>
  );
};