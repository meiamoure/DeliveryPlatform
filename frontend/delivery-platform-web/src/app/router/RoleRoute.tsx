import { Navigate } from 'react-router-dom';
import type { JSX } from 'react/jsx-dev-runtime';

export const RoleRoute = ({
  children,
  allowedRole,
}: {
  children: JSX.Element;
  allowedRole: 'Driver' | 'Dispatcher';
}) => {
  const role = localStorage.getItem('role');

  if (role !== allowedRole) {
    return <Navigate to="/" replace />;
  }

  return children;
};