import React from 'react';
import EmptyState from './EmptyState';

const DataTable = ({ columns, data, emptyMessage, className = "table table-hover align-middle mb-0" }) => (
  <div className="table-responsive">
    <table className={className}>
      <thead>
        <tr>
          {columns.map(column => (
            <th key={column.key} className={column.headerClassName}>
              {column.header}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {data.map(item => (
          <tr key={item.id}>
            {columns.map(column => (
              <td key={column.key} className={column.cellClassName}>
                {column.render ? column.render(item) : item[column.key]}
              </td>
            ))}
          </tr>
        ))}
        {data.length === 0 && (
          <EmptyState message={emptyMessage} colSpan={columns.length} />
        )}
      </tbody>
    </table>
  </div>
);

export default DataTable;
