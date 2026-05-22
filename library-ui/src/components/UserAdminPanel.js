import React from 'react';
import DataTable from './DataTable';

const roles = [
  { id: 1, name: 'Admin' },
  { id: 2, name: 'Staff' },
  { id: 3, name: 'Member' }
];

const UserAdminPanel = ({ users, onRoleChange }) => {
  const columns = [
    { key: "id", header: "ID" },
    { key: "fullName", header: "Ad Soyad", cellClassName: "fw-bold" },
    { key: "email", header: "E-posta" },
    { key: "roleName", header: "Rol" },
    {
      key: "roleChange",
      header: "Rol Degistir",
      headerClassName: "text-center",
      cellClassName: "text-center",
      render: user => (
        <select
          className="form-select form-select-sm mx-auto"
          style={{ maxWidth: 160 }}
          value={user.roleId}
          onChange={e => onRoleChange(user.id, Number(e.target.value))}
        >
          {roles.map(role => (
            <option key={role.id} value={role.id}>
              {role.name}
            </option>
          ))}
        </select>
      )
    }
  ];

  return (
    <div className="card shadow-sm border-0 mb-5">
      <div className="card-header bg-primary text-white fw-bold">Kullanici Yonetimi</div>
      <DataTable
        columns={columns}
        data={users}
        emptyMessage="Kullanici bulunamadi."
      />
    </div>
  );
};

export default UserAdminPanel;
