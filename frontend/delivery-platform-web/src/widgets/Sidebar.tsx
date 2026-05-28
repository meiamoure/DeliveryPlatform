import { NavLink } from 'react-router-dom';
import styles from './Sidebar.module.scss';

export const Sidebar = () => {
  return (
    <aside className={styles.sidebar}>
      <div className={styles.logoBlock}>
        <div className={styles.logoIcon}>L</div>

        <div>
          <div className={styles.logoTitle}>LogisticsPlanner</div>
          <div className={styles.logoSubtitle}>Route Planner</div>
        </div>
      </div>

      <nav className={styles.nav}>
        <NavLink
          to="/"
          className={({ isActive }) =>
            isActive
              ? `${styles.link} ${styles.activeLink}`
              : styles.link
          }
        >
          Панель
        </NavLink>

        <NavLink
          to="/deliveries"
          className={({ isActive }) =>
            isActive
              ? `${styles.link} ${styles.activeLink}`
              : styles.link
          }
        >
          Заявки / Точки
        </NavLink>

        <NavLink
          to="/vehicles"
          className={({ isActive }) =>
            isActive
              ? `${styles.link} ${styles.activeLink}`
              : styles.link
          }
        >
          Транспорт
        </NavLink>

          <NavLink
          to="/route-planning"
          className={({ isActive }) =>
            isActive
              ? `${styles.link} ${styles.activeLink}`
              : styles.link
          }
        >
          Розрахувати маршрути
        </NavLink>
        
        <NavLink
          to="/routes"
          className={({ isActive }) =>
            isActive
              ? `${styles.link} ${styles.activeLink}`
              : styles.link
          }
        >
          Маршрути
        </NavLink>

      </nav>

      <div>
        <div className={styles.userLabel}>User: Dispatcher</div>
        <div className={styles.userName}>Іван П.</div>
      </div>
    </aside>
  );
};