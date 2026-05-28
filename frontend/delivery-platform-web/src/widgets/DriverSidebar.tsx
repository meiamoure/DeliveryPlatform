import { NavLink } from 'react-router-dom';
import styles from './DriverSidebar.module.scss';

export const DriverSidebar = () => {
    return (
        <aside className={styles.sidebar}>
            <div className={styles.logoBlock}>
                <div className={styles.logoIcon}>D</div>
                <div>
                    <div className={styles.logoTitle}>Driver Panel</div>
                    <div className={styles.logoSubtitle}>Routes</div>
                </div>
            </div>

            <nav className={styles.nav}>
                <NavLink
                    to="/driver"
                    end
                    className={({ isActive }) =>
                        `${styles.link} ${isActive ? styles.activeLink : ''}`
                    }
                >
                    Мои маршруты
                </NavLink>

                <NavLink
                    to="/driver/history"
                    className={({ isActive }) =>
                        `${styles.link} ${isActive ? styles.activeLink : ''}`
                    }
                >
                    Історія маршрутів
                </NavLink>
            </nav>

            <div className={styles.userBlock}>
                <div className={styles.userLabel}>Вы вошли как</div>
                <div className={styles.userName}>Driver</div>
            </div>
        </aside>
    );
};