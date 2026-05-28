import { Sidebar } from '../../widgets/Sidebar';
import { Header } from '../../widgets/Header';
import styles from './DispatcherLayout.module.scss';

import { Outlet } from 'react-router-dom';

export const DispatcherLayout = () => {
  return (
    <div className={styles.app}>
      <Sidebar />

      <div className={styles.content}>
        <Header title="Панель" />

        <main className={styles.main}>
          <Outlet />
        </main>
      </div>
    </div>
  );
};