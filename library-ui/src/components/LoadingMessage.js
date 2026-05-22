import React from 'react';

const LoadingMessage = ({ message = "Veriler yenileniyor..." }) => (
  <div className="alert alert-info">{message}</div>
);

export default LoadingMessage;
