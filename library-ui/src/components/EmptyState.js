import React from 'react';

const EmptyState = ({ message = "Kayit bulunamadi.", colSpan = 1 }) => (
  <tr>
    <td colSpan={colSpan} className="text-center text-muted py-4">
      {message}
    </td>
  </tr>
);

export default EmptyState;
