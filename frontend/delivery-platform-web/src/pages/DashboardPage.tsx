import { useEffect, useState } from 'react';
import { getDashboardStats } from '../entities/dashboard/api';
import type { DashboardStats } from '../entities/dashboard/types';
import { StatCard } from '../widgets/StatCard';
import { CityMap } from '../widgets/CityMap';

const getTodayDate = () => {
    return new Date().toISOString().split('T')[0];
};

export const DashboardPage = () => {
    const [stats, setStats] = useState<DashboardStats | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const today = getTodayDate();

    useEffect(() => {
        const loadStats = async () => {
            try {
                setLoading(true);
                setError(null);

                const data = await getDashboardStats(today);
                setStats(data);
            } catch (e) {
                setError('Не удалось загрузить данные панели');
                console.error(e);
            } finally {
                setLoading(false);
            }
        };

        loadStats();
    }, [today]);

    if (loading) {
        return <div style={{ padding: 24 }}>Загрузка...</div>;
    }

    if (error) {
        return <div style={{ padding: 24, color: 'red' }}>{error}</div>;
    }

    if (!stats) {
        return <div style={{ padding: 24 }}>Нет данных</div>;
    }

    return (
        <div style={{ padding: 24 }}>
            <div
                style={{
                    display: 'grid',
                    gridTemplateColumns: 'repeat(3, minmax(0, 1fr))',
                    gap: 16,
                    marginBottom: 24,
                }}
            >
                <StatCard title="Заявок (на дату)" value={stats.pendingDeliveriesCount} />
                <StatCard title="Транспорт" value={stats.vehiclesCount} />
                <StatCard title="Спланировано маршрутов" value={stats.plannedRoutesCount} />
            </div>

            <div
                style={{
                    display: 'grid',
                    gridTemplateColumns: '1fr 420px',
                    gap: 16,
                }}
            >
                <div
                    style={{
                        background: '#fff',
                        border: '1px solid #e5e7eb',
                        borderRadius: 12,
                        padding: 20,
                        minHeight: 280,
                    }}
                >
                    <div style={{ fontWeight: 600, marginBottom: 12 }}>Карта</div>

                    <div
                        style={{
                            height: 300,
                            borderRadius: 12,
                            overflow: 'hidden',
                        }}
                    >
                        <CityMap />
                    </div>
                </div>

                <div
                    style={{
                        background: '#fff',
                        border: '1px solid #e5e7eb',
                        borderRadius: 12,
                        padding: 20,
                        minHeight: 280,
                    }}
                >
                    <div style={{ fontWeight: 600, marginBottom: 12 }}>Останні події</div>

                    <div style={{ color: '#6b7280', lineHeight: 1.8 }}>
                        <div>09:15 — Нова заявка D-007</div>
                        <div>09:05 — Авто AA-1002 готове до виїзду</div>
                        <div>08:00 — Оновлено статус заявки D-004 → Доставлено</div>
                    </div>
                </div>
            </div>
        </div>
    );
};