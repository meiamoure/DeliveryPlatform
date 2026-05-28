import { Outlet } from 'react-router-dom';

export const AuthLayout = () => {
  return (
    <div style={{ height: '100vh' }}>
      <Outlet />
    </div>
  );
};