import { Route, Routes } from 'react-router-dom';

import { DashboardPage } from '../../pages/DashboardPage';
import { DeliveriesPage } from '../../pages/DeliveriesPage';
import { VehiclesPage } from '../../pages/VehiclesPage';
import { RoutesPage } from '../../pages/RoutesPage/RoutesPage';
import { RoutePlanningPage } from '../../pages/RoutePlanningPage/RoutePlanningPage';
import { DriverPage } from '../../pages/DriverPage/DriverPage';
import { LoginPage } from '../../pages/LoginPage/LoginPage';

import { ProtectedRoute } from './ProtectedRoute';
import { RoleRoute } from './RoleRoute';

import { DispatcherLayout } from '../layouts/DispatcherLayout';
import { DriverLayout } from '../layouts/DriverLayout';
import { AuthLayout } from '../layouts/AuthLayout';
import { DriverHistoryPage } from '../../pages/DriverHistoryPage/DriverHistoryPage';

export const AppRouter = () => {
  return (
    <Routes>

      {/* LOGIN */}
      <Route element={<AuthLayout />}>
        <Route path="/login" element={<LoginPage />} />
      </Route>

      {/* DISPATCHER */}
      <Route
        element={
          <ProtectedRoute>
            <RoleRoute allowedRole="Dispatcher">
              <DispatcherLayout />
            </RoleRoute>
          </ProtectedRoute>
        }
      >
        <Route path="/" element={<DashboardPage />} />
        <Route path="/deliveries" element={<DeliveriesPage />} />
        <Route path="/vehicles" element={<VehiclesPage />} />
        <Route path="/routes" element={<RoutesPage />} />
        <Route path="/route-planning" element={<RoutePlanningPage />} />
      </Route>

      {/* DRIVER */}
      <Route
        path="/driver"
        element={
          <ProtectedRoute>
            <RoleRoute allowedRole="Driver">
              <DriverLayout />
            </RoleRoute>
          </ProtectedRoute>
        }
      >
        <Route index element={<DriverPage />} />
        <Route path="history" element={<DriverHistoryPage />} />
      </Route>

    </Routes>
  );
};