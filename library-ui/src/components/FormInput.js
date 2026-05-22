import React from 'react';

const FormInput = ({ label, type = "text", value, onChange, required = false, wrapperClassName = "col-md-4", ...props }) => (
  <div className={wrapperClassName}>
    <label className="form-label">{label}</label>
    <input
      type={type}
      className="form-control"
      value={value}
      onChange={onChange}
      required={required}
      {...props}
    />
  </div>
);

export default FormInput;
