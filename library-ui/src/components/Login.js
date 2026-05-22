import React, { useState } from 'react';
import axios from 'axios';

const API_URL = "https://localhost:64610/api";

function Login({ onLogin }) {
  const [mode, setMode] = useState("login");
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const resetFeedback = () => {
    setMessage("");
    setError("");
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    resetFeedback();

    try {
      if (mode === "register") {
        await axios.post(`${API_URL}/Auth/register`, { fullName, email, password });
        setMessage("Kayıt başarılı. Şimdi giriş yapabilirsin.");
        setMode("login");
        setPassword("");
        return;
      }

      const res = await axios.post(`${API_URL}/Auth/login`, { email, password });
      localStorage.setItem("token", res.data.token);
      onLogin();
    } catch (err) {
      setError(err.response?.data?.error || "İşlem başarısız.");
    }
  };

  return (
    <main className="auth-shell">
      <section className="auth-panel">
        <div>
          <p className="eyebrow">Kütüphane Yönetimi</p>
          <h1>{mode === "login" ? "Giriş Yap" : "Kayıt Ol"}</h1>
        </div>

        {message && <div className="alert alert-success">{message}</div>}
        {error && <div className="alert alert-danger">{error}</div>}

        <form onSubmit={handleSubmit} className="v-stack">
          {mode === "register" && (
            <label className="field">
              <span>Ad Soyad</span>
              <input
                type="text"
                className="form-control"
                value={fullName}
                onChange={e => setFullName(e.target.value)}
                required
              />
            </label>
          )}

          <label className="field">
            <span>E-posta</span>
            <input
              type="email"
              className="form-control"
              value={email}
              onChange={e => setEmail(e.target.value)}
              required
            />
          </label>

          <label className="field">
            <span>Şifre</span>
            <input
              type="password"
              className="form-control"
              value={password}
              onChange={e => setPassword(e.target.value)}
              required
              minLength={6}
            />
          </label>

          <button type="submit" className="btn btn-primary w-100">
            {mode === "login" ? "Giriş Yap" : "Kayıt Ol"}
          </button>
        </form>

        <button
          type="button"
          className="link-button"
          onClick={() => {
            resetFeedback();
            setMode(mode === "login" ? "register" : "login");
          }}
        >
          {mode === "login" ? "Hesabın yok mu? Kayıt ol" : "Hesabın var mı? Giriş yap"}
        </button>
      </section>
    </main>
  );
}

export default Login;
