import React, { useCallback, useEffect, useMemo, useState } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import Login from './components/Login';
import BookForm from './components/BookForm';
import BookList from './components/BookList';
import RentalList from './components/RentalList';
import UserAdminPanel from './components/UserAdminPanel';
import LoadingMessage from './components/LoadingMessage';

const API_URL = "https://localhost:64610/api";
const emptyBook = { title: "", author: "", stockCount: 1, isbn: "", photoUrl: "" };

function readTokenPayload(token) {
  try {
    return JSON.parse(atob(token.split('.')[1]));
  } catch {
    return null;
  }
}
// Token'dan kullanici rolunu alma
function getRoleFromToken(token) {
  const payload = readTokenPayload(token);
  return payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || null;
}

// Token'dan kullanici adini alma
function getNameFromToken(token) {
  const payload = readTokenPayload(token);
  return payload?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || "";
}

// Token'in gecerli olup olmadigini kontrol etme
function isTokenValid(token) {
  const payload = readTokenPayload(token);
  return Boolean(payload && (!payload.exp || payload.exp * 1000 > Date.now()));
}
//kullanici arayuzunde uygun yetkilendirme ve bilgi gosterimi yapilabilir.
function App() {
  const storedToken = localStorage.getItem("token");
  const [books, setBooks] = useState([]);
  const [bookPage, setBookPage] = useState(1);
  const [bookTotalPages, setBookTotalPages] = useState(1);
  const [rentals, setRentals] = useState([]);
  const [myRentals, setMyRentals] = useState([]);
  const [overdueRentals, setOverdueRentals] = useState([]);
  const [users, setUsers] = useState([]);
  const [borrowerSuggestions, setBorrowerSuggestions] = useState([]);
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [bookForm, setBookForm] = useState(emptyBook);
  const [editingBook, setEditingBook] = useState(null);
  const [bookSearch, setBookSearch] = useState("");
  const [overdueSearch, setOverdueSearch] = useState("");
  const [isLoggedIn, setIsLoggedIn] = useState(() => isTokenValid(storedToken));
  const [userRole, setUserRole] = useState(() => isTokenValid(storedToken) ? getRoleFromToken(storedToken) : null);
  const [userName, setUserName] = useState(() => isTokenValid(storedToken) ? getNameFromToken(storedToken) : "");
  const [activeTab, setActiveTab] = useState("books");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // Kullanici rolleri ve yetkilerine gore arayuzde gosterilecek sekmeleri ve islemleri belirleme
  const isAdmin = userRole === "Admin";
  const isStaff = userRole === "Staff";
  const isAdminOrStaff = isAdmin || isStaff;

  const tabs = useMemo(() => {
    const items = [{ id: "books", label: "Kitaplar" }];
    if (isAdmin) items.push({ id: "book-form", label: editingBook ? "Kitap Duzenle" : "Kitap Ekle" });
    if (isAdminOrStaff) {
      items.push({ id: "rentals", label: "Iade & Takip" });
      items.push({ id: "overdue", label: "Gecikenler" });
    }
    if (!isAdminOrStaff) items.push({ id: "my-rentals", label: "Gecmisim" });
    if (isAdmin) items.push({ id: "users", label: "Kullanicilar" });
    return items;
  }, [editingBook, isAdmin, isAdminOrStaff]);

  
  const clearAuth = useCallback(() => {
    localStorage.removeItem("token");
    delete axios.defaults.headers.common.Authorization;
    setIsLoggedIn(false);
    setUserRole(null);
    setUserName("");
    setBooks([]);
    setBookPage(1);
    setBookTotalPages(1);
    setBookSearch("");
    setRentals([]);
    setMyRentals([]);
    setOverdueRentals([]);
    setUsers([]);
    setBorrowerSuggestions([]);
    setActiveTab("books");
  }, []);

  const applyToken = useCallback((token) => {
    axios.defaults.headers.common.Authorization = `Bearer ${token}`;
    setUserRole(getRoleFromToken(token));
    setUserName(getNameFromToken(token));
    setIsLoggedIn(true);
  }, []);

  const handleRequestError = useCallback((err, fallbackMessage) => {
    if (err.response?.status === 401) {
      clearAuth();
      return;
    }

    alert(err.response?.data?.error || fallbackMessage);
  }, [clearAuth]);

  useEffect(() => {
    const interceptorId = axios.interceptors.response.use(
      response => response,
      error => {
        if (error.response?.status === 401) {
          clearAuth();
        }

        return Promise.reject(error);
      }
    );

    return () => axios.interceptors.response.eject(interceptorId);
  }, [clearAuth]);

  const getBooks = useCallback(async (search = "", page = 1) => {
    const res = await axios.get(`${API_URL}/Books`, {
      params: { page, pageSize: 10, search: search || undefined }
    });
    setBooks(res.data.items ?? res.data);
    setBookPage(res.data.page ?? page);
    setBookTotalPages(res.data.totalSize ?? 1);
  }, []);

  const getRentals = useCallback(async () => {
    const res = await axios.get(`${API_URL}/Rentals`);
    setRentals(res.data.items ?? res.data);
  }, []);

  const getOverdueRentals = useCallback(async (search = "") => {
    const res = await axios.get(`${API_URL}/Rentals/overdue`, {
      params: { page: 1, pageSize: 10, search: search || undefined }
    });
    setOverdueRentals(res.data.items ?? res.data);
  }, []);

  const getMyRentals = useCallback(async () => {
    const res = await axios.get(`${API_URL}/Rentals/my`);
    setMyRentals(res.data.items ?? res.data);
  }, []);

  const getUsers = useCallback(async (role) => {
    if (role !== "Admin") return;
    const res = await axios.get(`${API_URL}/Users`);
    setUsers(res.data.items ?? res.data);
  }, []);

  const searchBorrowers = useCallback(async (search) => {
    if (!search || search.trim().length < 2) {
      setBorrowerSuggestions([]);
      return;
    }

    const res = await axios.get(`${API_URL}/Users/search`, {
      params: { search }
    });

    setBorrowerSuggestions(res.data);
  }, []);

  const refreshData = useCallback(async (role, search) => {
    setLoading(true);
    setError("");
    try {
      await Promise.all([
        getBooks("", 1),
        role === "Member" ? getMyRentals() : Promise.resolve(),
        role === "Admin" || role === "Staff" ? getRentals() : Promise.resolve(),
        role === "Admin" || role === "Staff" ? getOverdueRentals(search) : Promise.resolve(),
        getUsers(role)
      ]);
    } catch (err) {
      if (err.response?.status === 401) {
        clearAuth();
        return;
      }

      setError(err.response?.data?.error || "Veriler alinamadi.");
    } finally {
      setLoading(false);
    }
  }, [clearAuth, getBooks, getMyRentals, getOverdueRentals, getRentals, getUsers]);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!isTokenValid(token)) {
      clearAuth();
      return;
    }

    const role = getRoleFromToken(token);
    applyToken(token);
    refreshData(role, "");
  }, [applyToken, clearAuth, refreshData]);

  useEffect(() => {
    if (!isLoggedIn || !isAdminOrStaff) return;

    getOverdueRentals(overdueSearch).catch(err => {
      if (err.response?.status !== 401) {
        setError(err.response?.data?.error || "Geciken kayitlar alinamadi.");
      }
    });
  }, [getOverdueRentals, isAdminOrStaff, isLoggedIn, overdueSearch]);

  useEffect(() => {
    if (!isLoggedIn || !isAdminOrStaff) return;

    const timeoutId = setTimeout(() => {
      searchBorrowers(fullName).catch(err => {
        if (err.response?.status !== 401) {
          setBorrowerSuggestions([]);
        }
      });
    }, 300);

    return () => clearTimeout(timeoutId);
  }, [fullName, isAdminOrStaff, isLoggedIn, searchBorrowers]);

  useEffect(() => {
    if (!isLoggedIn) return;

    const timeoutId = setTimeout(() => {
      getBooks(bookSearch, bookPage).catch(err => {
        if (err.response?.status !== 401) {
          setError(err.response?.data?.error || "Kitaplar alinamadi.");
        }
      });
    }, 300);

    return () => clearTimeout(timeoutId);
  }, [bookPage, bookSearch, getBooks, isLoggedIn]);

  const handleLoginSuccess = async () => {
    const token = localStorage.getItem("token");
    if (!isTokenValid(token)) {
      clearAuth();
      return;
    }

    const role = getRoleFromToken(token);
    applyToken(token);
    await refreshData(role, overdueSearch);
  };

  const saveBook = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("Title", bookForm.title);
    formData.append("Author", bookForm.author);
    formData.append("ISBN", bookForm.isbn);
    formData.append("StockCount", bookForm.stockCount);
    formData.append("PhotoUrl", bookForm.photoUrl || "");

    if (bookForm.photo instanceof File) {
      formData.append("Photo", bookForm.photo);
    }

    try {
      if (editingBook) {
        await axios.put(`${API_URL}/Books/${editingBook.id}`, formData);
      } else {
        await axios.post(`${API_URL}/Books`, formData);
      }

      setBookForm(emptyBook);
      setEditingBook(null);
      await getBooks(bookSearch, bookPage);
      setActiveTab("books");
    } catch (err) {
      handleRequestError(err, "Kitap kaydedilemedi.");
    }
  };

  const startEditBook = (book) => {
    setEditingBook(book);
    setBookForm({
      title: book.title,
      author: book.author,
      stockCount: book.stockCount,
      isbn: book.isbn,
      photoUrl: book.photoUrl || ""
    });
    setActiveTab("book-form");
  };

  const cancelBookForm = () => {
    setEditingBook(null);
    setBookForm(emptyBook);
    setActiveTab("books");
  };

  const rentBook = async (bookId) => {
    if (!fullName || !email) {
      alert("Kiralayan ad soyad ve e-posta alanlarini doldur.");
      return;
    }

    try {
      await axios.post(`${API_URL}/Rentals/rent`, {
        fullName,
        email,
        bookId: Number(bookId)
      });
      setFullName("");
      setEmail("");
      setBorrowerSuggestions([]);
      await refreshData(userRole, overdueSearch);
      setActiveTab("rentals");
    } catch (err) {
      handleRequestError(err, "Kiralama yapilamadi.");
    }
  };

  const returnBook = async (rentalId) => {
    try {
      await axios.post(`${API_URL}/Rentals/return/${rentalId}`);
      await refreshData(userRole, overdueSearch);
    } catch (err) {
      handleRequestError(err, "Iade islemi yapilamadi.");
    }
  };

  const deleteBook = async (bookId) => {
    if (!window.confirm("Bu kitabi silmek istedigine emin misin?")) return;

    try {
      await axios.delete(`${API_URL}/Books/${bookId}`);
      await getBooks(bookSearch, bookPage);
    } catch (err) {
      handleRequestError(err, "Kitap silinemedi.");
    }
  };

  const updateUserRole = async (userId, roleId) => {
    try {
      await axios.put(`${API_URL}/Users/${userId}/role`, { roleId });
      await getUsers("Admin");
    } catch (err) {
      handleRequestError(err, "Rol guncellenemedi.");
    }
  };

  if (!isLoggedIn) {
    return <Login onLogin={handleLoginSuccess} />;
  }

  return (
    <main className="app-shell">
      <aside className="sidebar">
        <div>
          <p className="eyebrow">Kutuphane Sistemi</p>
          <h1>Yonetim Paneli</h1>
          <p className="muted mb-0">{userName || "Kullanici"} - {userRole || "Rol yok"}</p>
        </div>

        <nav className="nav-list">
          {tabs.map(tab => (
            <button
              key={tab.id}
              className={activeTab === tab.id ? "active" : ""}
              onClick={() => setActiveTab(tab.id)}
            >
              {tab.label}
            </button>
          ))}
        </nav>

        <button className="btn btn-outline-secondary w-100" onClick={clearAuth}>
          Cikis Yap
        </button>
      </aside>

      <section className="content">
        {error && <div className="alert alert-danger">{error}</div>}
        {loading && <LoadingMessage />}

        {activeTab === "books" && (
          <>
            <div className="section-heading">
              <div>
                <h2>Kitaplar</h2>
                <p>Stok durumunu takip et, yetkin varsa kiralama veya yonetim islemlerini yap.</p>
              </div>
              <input
                type="search"
                className="form-control search-input"
                placeholder="Kitap, yazar veya ISBN ara"
                value={bookSearch}
                onChange={e => {
                  setBookSearch(e.target.value);
                  setBookPage(1);
                }}
              />
              {isAdmin && (
                <button className="btn btn-primary" onClick={() => setActiveTab("book-form")}>
                  Kitap Ekle
                </button>
              )}
            </div>

            {isAdminOrStaff && (
              <div className="panel mb-4">
                <h3>Kiralayan Bilgileri</h3>
                <div className="row g-3">
                  <div className="col-md-6 position-relative">
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Ad Soyad"
                      value={fullName}
                      onChange={e => setFullName(e.target.value)}
                    />
                    {borrowerSuggestions.length > 0 && (
                      <div className="list-group position-absolute w-100 shadow-sm" style={{ zIndex: 10 }}>
                        {borrowerSuggestions.map(user => (
                          <button
                            key={user.id}
                            type="button"
                            className="list-group-item list-group-item-action"
                            onClick={() => {
                              setFullName(user.fullName);
                              setEmail(user.email);
                              setBorrowerSuggestions([]);
                            }}
                          >
                            <span className="fw-semibold">{user.fullName}</span>
                            <span className="text-muted ms-2">{user.email}</span>
                          </button>
                        ))}
                      </div>
                    )}
                  </div>
                  <div className="col-md-6">
                    <input
                      type="email"
                      className="form-control"
                      placeholder="E-posta"
                      value={email}
                      onChange={e => setEmail(e.target.value)}
                    />
                  </div>
                </div>
              </div>
            )}

            <div className="panel">
              <BookList
                books={books}
                onRent={isAdminOrStaff ? rentBook : null}
                onEdit={isAdmin ? startEditBook : null}
                onDelete={isAdmin ? deleteBook : null}
              />
              <div className="d-flex align-items-center justify-content-between mt-3">
                <button
                  type="button"
                  className="btn btn-outline-secondary btn-sm"
                  disabled={bookPage <= 1}
                  onClick={() => setBookPage(page => Math.max(1, page - 1))}
                >
                  Onceki
                </button>
                <span className="text-muted small">
                  Sayfa {bookPage} / {bookTotalPages}
                </span>
                <button
                  type="button"
                  className="btn btn-outline-secondary btn-sm"
                  disabled={bookPage >= bookTotalPages}
                  onClick={() => setBookPage(page => Math.min(bookTotalPages, page + 1))}
                >
                  Sonraki
                </button>
              </div>
            </div>
          </>
        )}

        {activeTab === "book-form" && isAdmin && (
          <div className="panel">
            <div className="section-heading compact">
              <div>
                <h2>{editingBook ? "Kitap Duzenle" : "Kitap Ekle"}</h2>
                <p>Backend'deki Books endpoint'i ile dogrudan calisir.</p>
              </div>
            </div>
            <BookForm
              book={bookForm}
              setBook={setBookForm}
              onSubmit={saveBook}
              onCancel={cancelBookForm}
              submitLabel={editingBook ? "Guncelle" : "Kaydet"}
            />
          </div>
        )}

        {activeTab === "rentals" && (
          <>
            <div className="section-heading">
              <div>
                <h2>Iade & Takip</h2>
                <p>Tum kiralama kayitlari ve iade islemleri.</p>
              </div>
            </div>
            <RentalList rentals={rentals} onReturn={isAdminOrStaff ? returnBook : null} />
          </>
        )}

        {activeTab === "overdue" && (
          <>
            <div className="section-heading">
              <div>
                <h2>Gecikenler</h2>
                <p>Iade tarihi gecmis kayitlari kitap veya kullanici adina gore ara.</p>
              </div>
              <input
                type="search"
                className="form-control search-input"
                placeholder="Kitap veya kullanici ara"
                value={overdueSearch}
                onChange={e => setOverdueSearch(e.target.value)}
              />
            </div>
            <RentalList rentals={overdueRentals} onReturn={isAdminOrStaff ? returnBook : null} />
          </>
        )}

        {activeTab === "my-rentals" && (
          <>
            <div className="section-heading">
              <div>
                <h2>Gecmisim</h2>
                <p>Kendi kiralama gecmisini goruntule.</p>
              </div>
            </div>
            <RentalList rentals={myRentals} onReturn={null} />
          </>
        )}

        {activeTab === "users" && isAdmin && (
          <>
            <div className="section-heading">
              <div>
                <h2>Kullanicilar</h2>
                <p>Kullanici rollerini Admin, Staff veya Member olarak guncelle.</p>
              </div>
            </div>
            <UserAdminPanel users={users} onRoleChange={updateUserRole} />
          </>
        )}
      </section>
    </main>
  );
}

export default App;
