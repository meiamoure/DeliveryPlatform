import { useEffect, useState } from 'react';
import { baseApi } from '../../shared/api/baseApi';

export type DriverRouteDto = {
  id: string;
  vehiclePlate: string;
  serviceDate: string;
  status: 'Planned' | 'Accepted' | 'InProgress' | 'Completed';

  orderedPoints: {
    nodeId: string;
    lat: number;
    lng: number;
  }[];

  path: {
    lat: number;
    lng: number;
  }[];

  stops: {
    sequence: number;
    nodeId: string;
    nodeName: string;
    lat: number;
    lng: number;
    orderNumber: string | null;
  }[];

  totalDistanceKm: number;
  totalTimeMin: number;

  number: number;
  code: string;
};

export const useDriverRoute = () => {
  const [route, setRoute] = useState<DriverRouteDto | null>(null);
  const [loading, setLoading] = useState(true);

  const load = async () => {
    try {
      const res = await baseApi.get('/driver/my-route');
      setRoute(res.data);
    } catch {
      setRoute(null);
    } finally {
      setLoading(false);
    }
  };

  const accept = async () => {
    if (!route) return;
    await baseApi.post(`/driver/${route.id}/accept`);
    await load();
  };

  const start = async () => {
    if (!route) return;
    await baseApi.post(`/driver/${route.id}/start`);
    await load();
  };

  const complete = async () => {
    if (!route) return;
    await baseApi.post(`/driver/${route.id}/complete`);
    await load();
  };

  useEffect(() => {
    load();
  }, []);

  return {
    route,
    loading,
    accept,
    start,
    complete,
    reload: load,
  };
};