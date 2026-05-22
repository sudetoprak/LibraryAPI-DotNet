import React from 'react';
import DataTable from './DataTable';

const API_ORIGIN = "https://localhost:64610";

const BookList = ({ books, onRent, onDelete, onEdit }) => {
  const columns = [
    { key: "id", header: "ID" },
    {
      key: "photoUrl",
      header: "Fotograf",
      render: book => book.photoUrl && (
        <img
          src={`${API_ORIGIN}${book.photoUrl}`}
          alt={book.title}
          style={{ width: "50px", height: "75px", objectFit: "cover" }}
        />
      )
    },
    { key: "title", header: "Kitap Basligi", cellClassName: "fw-semibold" },
    { key: "author", header: "Yazar" },
    { key: "isbn", header: "ISBN" },
    {
      key: "stockCount",
      header: "Stok",
      render: book => (
        <span className={`badge ${book.stockCount > 0 ? "text-bg-success" : "text-bg-danger"}`}>
          {book.stockCount}
        </span>
      )
    },
    {
      key: "actions",
      header: "Islem",
      headerClassName: "text-end",
      render: book => (
        <div className="d-flex gap-2 justify-content-end">
          {onRent && (
            <button
              onClick={() => onRent(book.id)}
              className="btn btn-sm btn-primary"
              disabled={book.stockCount === 0}
            >
              Kirala
            </button>
          )}

          {onEdit && (
            <button onClick={() => onEdit(book)} className="btn btn-sm btn-outline-secondary">
              Duzenle
            </button>
          )}

          {onDelete && (
            <button onClick={() => onDelete(book.id)} className="btn btn-sm btn-outline-danger">
              Sil
            </button>
          )}
        </div>
      )
    }
  ];

  return (
    <DataTable
      columns={columns}
      data={books}
      emptyMessage="Kayit bulunamadi."
    />
  );
};

export default BookList;
