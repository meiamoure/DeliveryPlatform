import { useEffect, useState } from 'react';
import { routeApi } from '../../entities/route/api';
import {
  routeStatusLabels,
  type RouteDetailsDto,
  type RouteListItemDto,
  type RouteSegmentDetailsDto,
} from '../../entities/route/types';
import styles from './RoutesPage.module.scss';
import { RouteMap } from '../../widgets/RouteMap';
import { formatTime } from '../../shared/lib/formatTime';

export const RoutesPage = () => {
  const [routes, setRoutes] = useState<RouteListItemDto[]>([]);
  const [selectedRouteId, setSelectedRouteId] = useState<string | null>(null);
  const [selectedRoute, setSelectedRoute] = useState<RouteDetailsDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const loadRoutes = async () => {
      try {
        setIsLoading(true);
        const data = await routeApi.getRoutes();
        setRoutes(data);

        if (data.length > 0) {
          setSelectedRouteId(data[0].id);
        }
      } finally {
        setIsLoading(false);
      }
    };

    void loadRoutes();
  }, []);

  useEffect(() => {
    if (!selectedRouteId) return;

    const loadDetails = async () => {
      const details = await routeApi.getRouteById(selectedRouteId);
      setSelectedRoute(details);
    };

    void loadDetails();
  }, [selectedRouteId]);

  if (isLoading) {
    return <div className={styles.page}><div className={styles.message}>Загрузка маршрутов...</div></div>;
  }

  return (
    <div className={styles.page}>
      <div className={styles.topBar}>
        <div>
          <h1 className={styles.title}>Маршрути</h1>
          <p className={styles.subtitle}>
            Перегляд побудованих маршрутів, сегментів та точок руху. Виберіть маршрут для перегляду деталей та його розташування на карті.
          </p>
        </div>
      </div>

      <div className={styles.layout}>
        <div className={styles.panel}>
          <h2 className={styles.panelTitle}>Список маршрутів</h2>

          {routes.length === 0 ? (
            <div className={styles.empty}>Маршрутів поки що немає</div>
          ) : (
            routes.map((route) => (
              <div
                key={route.id}
                onClick={() => setSelectedRouteId(route.id)}
                className={`${styles.routeCard} ${selectedRouteId === route.id ? styles.routeCardActive : ''}`}
              >
                <div className={styles.routeId}>
                  Маршрут №{route.number}
                </div>

                <div className={styles.routeMeta}>
                  {route.code} • {route.vehiclePlate}
                </div>
                <div className={styles.routeMeta}>Транспорт: {route.vehiclePlate}</div>
                <div className={styles.routeMeta}>Дата: {route.serviceDate}</div>
                <div className={styles.routeMeta}>
                  Статус: {routeStatusLabels[route.status] ?? route.status}
                </div>
                <div className={styles.routeMeta}>
                  {Number(route.totalDistanceKm.toFixed(2))} км • {formatTime(route.totalTimeMin)}
                </div>
              </div>
            ))
          )}
        </div>

        <div className={styles.panel}>
          <h2 className={styles.panelTitle}>Карта</h2>
          <RouteMap route={selectedRoute} />
        </div>

        <div className={styles.panel}>
          <h2 className={styles.panelTitle}>Деталі маршруту</h2>

          {!selectedRoute ? (
            <div className={styles.empty}>Виберіть маршрут</div>
          ) : (
            <>
              <div className={styles.detailsList}>
                <div className={styles.detailItem}>
                  <div className={styles.detailItem}>
                    <span>Маршрут</span>
                    <strong>{selectedRoute.code}</strong>
                  </div>
                </div>

                <div className={styles.detailItem}>
                  <span>Дата</span>
                  <strong>{selectedRoute.serviceDate}</strong>
                </div>

                <div className={styles.detailItem}>
                  <span>Транспорт</span>
                  <strong>{selectedRoute.vehiclePlate}</strong>
                </div>

                <div className={styles.detailItem}>
                  <span>Водій</span>
                  <strong>
                    {selectedRoute.driverName ?? 'Не назначен'}
                  </strong>
                </div>

                <div className={styles.detailItem}>
                  <span>Статус</span>
                  <strong>{routeStatusLabels[selectedRoute.status] ?? selectedRoute.status}</strong>
                </div>

                <div className={styles.detailItem}>
                  <span>Дистанція</span>
                  <strong>{Number(selectedRoute.totalDistanceKm.toFixed(2))} км</strong>
                </div>

                <div className={styles.detailItem}>
                  <span>Час</span>
                  <strong>{formatTime(selectedRoute.totalTimeMin)}</strong>
                </div>
              </div>

              <h3 className={styles.sectionTitle}>Сегменти</h3>
              {selectedRoute.segments.length === 0 ? (
                <div className={styles.empty}>Сегментів немає</div>
              ) : (
                selectedRoute.segments.map((segment: RouteSegmentDetailsDto) => (
                  <div
                    key={`${segment.order}-${segment.fromNodeId}-${segment.toNodeId}`}
                    className={styles.segmentCard}
                  >
                    <div className={styles.segmentOrder}>#{segment.order}</div>
                    <div className={styles.segmentText}>
                      {segment.fromNodeName} → {segment.toNodeName}
                    </div>
                    <div className={styles.segmentMeta}>
                      {formatTime(segment.travelTimeMin)} / {Number(segment.distanceKm.toFixed(2))} км
                    </div>
                    <div className={styles.segmentMeta}>
                    </div>
                  </div>
                ))
              )}

              <h3 className={styles.sectionTitle}>Точки маршрута</h3>
              {selectedRoute.orderedPoints.length === 0 ? (
                <div className={styles.empty}>Точок немає</div>
              ) : (
                selectedRoute.orderedPoints.map((point, index) => (
                  <div
                    key={`${point.nodeId}-${index}`}
                    className={styles.pointCard}
                  >
                    <div className={styles.pointOrder}>Точка {index + 1}</div>
                    <div className={styles.pointText}>ID вузла: {point.nodeId}</div>
                    <div className={styles.pointMeta}>
                      Координати: {point.lat}, {point.lng}
                    </div>
                  </div>
                ))
              )}
            </>
          )}
        </div>
      </div>
    </div>
  );
};