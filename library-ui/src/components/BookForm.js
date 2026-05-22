import React from 'react';
import FormInput from './FormInput';

const BookForm = ({ book, setBook, onSubmit, onCancel, submitLabel = "Kaydet" }) => (
  <form onSubmit={onSubmit} className="row g-3">
    <FormInput
      label="Kitap Basligi"
      value={book.title}
      onChange={e => setBook({ ...book, title: e.target.value })}
      required
    />

    <FormInput
      label="Yazar"
      value={book.author}
      onChange={e => setBook({ ...book, author: e.target.value })}
      wrapperClassName="col-md-3"
      required
    />

    <FormInput
      label="Stok"
      type="number"
      min={0}
      value={book.stockCount}
      onChange={e => setBook({ ...book, stockCount: Number(e.target.value) })}
      wrapperClassName="col-md-2"
      required
    />

    <FormInput
      label="ISBN"
      value={book.isbn}
      onChange={e => setBook({ ...book, isbn: e.target.value })}
      wrapperClassName="col-md-3"
      required
    />

    <div className="col-md-4">
      <label className="form-label">Fotograf</label>
      <input
        type="file"
        className="form-control"
        accept="image/*"
        onChange={e => setBook({ ...book, photo: e.target.files[0] })}
      />
    </div>

    <div className="col-12 d-flex gap-2">
      <button type="submit" className="btn btn-primary">{submitLabel}</button>
      {onCancel && (
        <button type="button" className="btn btn-outline-secondary" onClick={onCancel}>
          Vazgec
        </button>
      )}
    </div>
  </form>
);

export default BookForm;
