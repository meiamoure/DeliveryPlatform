import { useState } from 'react';
import { baseApi } from '../../shared/api/baseApi';
import { useNavigate } from 'react-router-dom';
import styles from './LoginPage.module.scss';

export const LoginPage = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async () => {
    const res = await baseApi.post('/auth/login', { login, password });

    localStorage.setItem('token', res.data.accessToken);
    localStorage.setItem('role', res.data.role);

    if (res.data.role === 'Driver') {
      navigate('/driver');
    } else {
      navigate('/');
    }
  };

  return (
    <div className={styles.page}>
      <div className={styles.card}>
        <h1>LogisticsPlanner</h1>
        <p>Route Planning System</p>

        <input
          placeholder="Логин"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
        />

        <input
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <button onClick={handleLogin}>Войти</button>
      </div>
    </div>
  );
};