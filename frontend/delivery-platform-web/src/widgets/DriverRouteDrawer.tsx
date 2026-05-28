import styles from './DriverRouteDrawer.module.scss';

type Props = {
  route: any;
  onClose: () => void;
};

export const DriverRouteDrawer = ({ route, onClose }: Props) => {
  if (!route) return null;

  return (
    <div className={styles.overlay}>
      <div className={styles.drawer}>
        <div className={styles.header}>
          <h3>Маршрут №{route.number}</h3>
          <button onClick={onClose}>✕</button>
        </div>

        <div className={styles.meta}>
          <div>📅 {route.serviceDate}</div>
          <div>📏 {route.totalDistanceKm.toFixed(1)} км</div>
          <div>⏱ {route.totalTimeMin} хв</div>
        </div>

        <div className={styles.map}>
          <div className={styles.mapPlaceholder}>
            Карта тут (пока можно позже подключить)
          </div>
        </div>

        <div className={styles.stops}>
          <h4>Зупинки</h4>

          {route.stops?.map((s: any, i: number) => (
            <div key={i} className={styles.stop}>
              <div className={styles.index}>{i + 1}</div>
              <div>
                <div>{s.nodeName}</div>
                {s.orderNumber && (
                  <div className={styles.order}>{s.orderNumber}</div>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};