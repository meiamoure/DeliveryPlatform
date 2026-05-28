import { useEffect } from 'react';
import L from 'leaflet';
import {
    MapContainer,
    Marker,
    Polyline,
    Popup,
    TileLayer,
    useMap,
} from 'react-leaflet';
import type { RouteDetailsDto } from '../entities/route/types';

type Props = {
    route: RouteDetailsDto | null;
};

type FitBoundsProps = {
    positions: [number, number][];
};

const FitBounds = ({ positions }: FitBoundsProps) => {
    const map = useMap();

    useEffect(() => {
        if (positions.length === 0) return;

        const bounds = L.latLngBounds(positions);
        map.fitBounds(bounds, { padding: [30, 30] });
    }, [map, positions]);

    return null;
};

const createPointIcon = (color: 'green' | 'red' | 'blue', label: string) =>
    L.divIcon({
        className: '',
        html: `
      <div style="
        width: 28px;
        height: 28px;
        border-radius: 50%;
        background: ${color};
        border: 3px solid white;
        box-shadow: 0 2px 8px rgba(0,0,0,0.25);
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        font-size: 12px;
        font-weight: 700;
      ">
        ${label}
      </div>
    `,
        iconSize: [28, 28],
        iconAnchor: [14, 14],
        popupAnchor: [0, -14],
    });

const getMarkerIcon = (index: number, total: number) => {
    if (index === 0) {
        return createPointIcon('green', 'S');
    }

    if (index === total - 1) {
        return createPointIcon('red', 'F');
    }

    return createPointIcon('blue', String(index));
};

export const RouteMap = ({ route }: Props) => {
    if (!route || route.orderedPoints.length === 0) {
        return (
            <div
                style={{
                    minHeight: '100%',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                Маршрут не выбран
            </div>
        );
    }

    const positions: [number, number][] =
        route.path.length > 0
            ? route.path.map((point) => [point.lat, point.lng])
            : route.orderedPoints.map((point) => [point.lat, point.lng]);

    const fallbackCenter: [number, number] = [
        route.orderedPoints[0].lat,
        route.orderedPoints[0].lng,
    ];

    return (
        <MapContainer
            center={fallbackCenter}
            zoom={12}
            style={{ height: '100%', width: '100%', borderRadius: '12px' }}
            scrollWheelZoom
        >
            <TileLayer
                attribution="&copy; OpenStreetMap contributors"
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />

            <FitBounds positions={positions} />

            <Polyline positions={positions} />

            {route.stops.map((stop, index) => (
                <Marker
                    key={`${stop.nodeId}-${index}`}
                    position={[stop.lat, stop.lng]}
                    icon={getMarkerIcon(index, route.stops.length)}
                >
                    <Popup>
                        <strong>{stop.nodeName}</strong>
                        {stop.orderNumber && (
                            <>
                                <br />
                                Замовлення: {stop.orderNumber}
                            </>
                        )}
                    </Popup>
                </Marker>
            ))}
        </MapContainer>
    );
};