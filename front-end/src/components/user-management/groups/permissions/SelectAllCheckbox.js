import React, { useRef, useEffect } from 'react';
import { Form } from 'react-bootstrap';

const SelectAllCheckbox = ({
  checked,
  indeterminate,
  onChange,
  disabled = false
}) => {
  const checkboxRef = useRef(null);

  useEffect(() => {
    if (checkboxRef.current) {
      checkboxRef.current.indeterminate = indeterminate;
    }
  }, [indeterminate]);

  return (
    <Form.Check
      ref={checkboxRef}
      checked={checked}
      onChange={onChange}
      disabled={disabled}
    />
  );
};

export default SelectAllCheckbox;
