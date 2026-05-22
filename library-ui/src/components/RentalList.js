import React from 'react';
import DataTable from './DataTable';

const formatDate = (date) => {
  if (!date) return "-";
  return new Date(date).toLocaleDateString();
};

const RentalList = ({ rentals, onReturn }) => {
  const columns = [
    { key: "userName", header: "Kullanici", render: rental => rental.userName ?? rental.fullName },
    { key: "bookTitle", header: "Kitap" },
    { key: "rentalDate", header: "Kiralama Tarihi", render: rental => formatDate(rental.rentalDate) },
    { key: "dueDate", header: "Teslim Tarihi", render: rental => formatDate(rental.dueDate) },
    {
      key: "status",
      header: "Durum",
      render: rental => rental.isReturned
        ? <span className="text-muted">Iade Edildi</span>
        : <span className="text-warning fw-bold">Emanet edildi</span>
    },
    {
      key: "actions",
      header: "Eylem",
      render: rental => (
        <>
          {!rental.isReturned && onReturn && (
            <button onClick={() => onReturn(rental.id)} className="btn btn-sm btn-outline-danger py-0">
              Iade Et
            </button>
          )}
          {!rental.isReturned && !onReturn && <span className="text-muted small">Yetki yok</span>}
        </>
      )
    }
  ];

  return (
    <div className="card shadow-sm border-0">
      <div className="card-header bg-dark text-white fw-bold">Kiralamalar & Iade</div>
      <DataTable
        columns={columns}
        data={rentals}
        emptyMessage="Kiralama kaydi bulunamadi."
        className="table table-sm mb-0"
      />
    </div>
  );
};

export default RentalList;
