import styles from './Header.module.scss';

type Props = {
  title: string;
};

export const Header = ({ title }: Props) => {
  const today = new Date().toISOString().split('T')[0];

  const role = localStorage.getItem('role');

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    window.location.href = '/login';
  };

  return (
    <header className={styles.header}>
      <h1 className={styles.title}>{title}</h1>

      <div className={styles.right}>
        <div className={styles.date}>
          Дата: <strong>{today}</strong>
        </div>

        <div className={styles.role}>
          {role}
        </div>

        <button className={styles.logout} onClick={logout}>
          Выйти
        </button>

        <div className={styles.avatar}>IP</div>
      </div>
    </header>
  );
};