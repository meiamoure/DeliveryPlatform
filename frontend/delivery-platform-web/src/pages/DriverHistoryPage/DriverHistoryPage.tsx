import { useState } from 'react';
import { useDriverHistory } from '../../features/driver/useDriverHistory';
import styles from './DriverHistoryPage.module.scss';
import { formatTime } from '../../shared/lib/formatTime';
import { routeApi } from '../../entities/route/api';
import type { RouteDetailsDto } from '../../entities/route/types';
import { RouteMap } from '../../widgets/RouteMap';
import { Calendar, Route, Clock } from 'lucide-react';

export const DriverHistoryPage = () => {
    const { data, loading } = useDriverHistory();

    const [selectedRouteId, setSelectedRouteId] = useState<string | null>(null);
    const [details, setDetails] = useState<RouteDetailsDto | null>(null);
    const [loadingDetails, setLoadingDetails] = useState(false);

    const openDetails = async (routeId: string) => {
        try {
            setSelectedRouteId(routeId);
            setLoadingDetails(true);

            const res = await routeApi.getRouteById(routeId);
            setDetails(res);
        } catch (e) {
            console.error(e);
            setDetails(null);
        } finally {
            setLoadingDetails(false);
        }
    };

    const closeDrawer = () => {
        setSelectedRouteId(null);
        setDetails(null);
    };

    if (loading) {
        return <div className={styles.page}>Завантаження...</div>;
    }

    if (!data || data.length === 0) {
        return (
            <div className={styles.page}>
                <div className={styles.empty}>Історія маршрутів порожня</div>
            </div>
        );
    }

    return (
        <div className={styles.page}>
            <h1 className={styles.title}>Історія маршрутів</h1>

            {/* СПИСОК */}
            <div className={styles.list}>
                {data.map((route) => (
                    <div key={route.id} className={styles.card}>

                        {/* HEADER */}
                        <div className={styles.header}>
                            <div className={styles.number}>
                                Маршрут №{route.number}
                            </div>
                            <div className={styles.code}>{route.code}</div>
                        </div>

                        {/* НИЖНЯЯ СТРОКА (ВАЖНО) */}
                        <div className={styles.bottom}>

                            <div className={styles.meta}>
                                <div className={styles.metaItem}>
                                    <div className={styles.iconWrapper}>
                                        <Calendar size={14} />
                                    </div>
                                    <span>{route.serviceDate}</span>
                                </div>

                                <div className={styles.divider} />

                                <div className={styles.metaItem}>
                                    <div className={styles.iconWrapper}>
                                        <Route size={14} />
                                    </div>
                                    <span>{route.totalDistanceKm.toFixed(1)} км</span>
                                </div>

                                <div className={styles.divider} />

                                <div className={styles.metaItem}>
                                    <div className={styles.iconWrapper}>
                                        <Clock size={14} />
                                    </div>
                                    <span>{formatTime(route.totalTimeMin)}</span>
                                </div>
                            </div>

                            <button
                                className={styles.detailsButton}
                                onClick={() => openDetails(route.id)}
                            >
                                Деталі
                            </button>

                        </div>

                    </div>
                ))}
            </div>

            {/* DRAWER */}
            {selectedRouteId && (
                <div
                    className={styles.drawerOverlay}
                    onClick={closeDrawer}
                >
                    <div
                        className={styles.drawer}
                        onClick={(e) => e.stopPropagation()}
                    >
                        {loadingDetails && (
                            <div className={styles.empty}>Завантаження маршруту...</div>
                        )}

                        {!loadingDetails && details && (
                            <>
                                <div className={styles.drawerHeader}>
                                    <div>
                                        <div className={styles.drawerTitle}>
                                            Маршрут №{details.number}
                                        </div>
                                        <div className={styles.drawerCode}>
                                            {details.code}
                                        </div>
                                    </div>

                                    <button
                                        className={styles.closeButton}
                                        onClick={closeDrawer}
                                    >
                                        ✕
                                    </button>
                                </div>

                                <div className={styles.drawerMeta}>
                                    <span>{details.serviceDate}</span>
                                    <span>{details.vehiclePlate}</span>
                                    <span>{details.totalDistanceKm.toFixed(1)} км</span>
                                    <span>{formatTime(details.totalTimeMin)}</span>
                                </div>

                                <div className={styles.map}>
                                    <RouteMap route={details} />
                                </div>

                                <div className={styles.stops}>
                                    <h3>Зупинки</h3>

                                    {details.stops.length ? (
                                        details.stops.map((stop, index) => (
                                            <div key={index} className={styles.stop}>
                                                <div className={styles.stopIndex}>
                                                    {index + 1}
                                                </div>

                                                <div className={styles.stopInfo}>
                                                    <div className={styles.stopName}>
                                                        {stop.nodeName}
                                                    </div>

                                                    {stop.orderNumber && (
                                                        <div className={styles.stopOrder}>
                                                            {stop.orderNumber}
                                                        </div>
                                                    )}
                                                </div>
                                            </div>
                                        ))
                                    ) : (
                                        <div className={styles.empty}>Немає зупинок</div>
                                    )}
                                </div>
                            </>
                        )}

                        {!loadingDetails && !details && (
                            <div className={styles.empty}>
                                Не вдалося завантажити маршрут
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};