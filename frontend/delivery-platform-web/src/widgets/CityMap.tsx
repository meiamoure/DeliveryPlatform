import { MapContainer, TileLayer } from 'react-leaflet';

export const CityMap = () => {
  const kharkivCenter: [number, number] = [49.9935, 36.2304];

  return (
    <MapContainer
      center={kharkivCenter}
      zoom={12}
      style={{ height: '100%', width: '100%', borderRadius: '12px' }}
    >
      <TileLayer
        attribution='&copy; OpenStreetMap contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
    </MapContainer>
  );
};