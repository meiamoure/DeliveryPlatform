import { Header } from '../../widgets/Header';
import styles from './DriverLayout.module.scss';
import { Outlet } from 'react-router-dom';
import { DriverSidebar } from '../../widgets/DriverSidebar';

export const DriverLayout = () => {
  return (
    <div className={styles.app}>
      <DriverSidebar />

      <div className={styles.content}>
        <Header title="Мои маршруты" />

        <main className={styles.main}>
          <Outlet />
        </main>
      </div>
    </div>
  );
};