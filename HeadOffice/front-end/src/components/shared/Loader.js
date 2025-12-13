import React from 'react';
import { Spinner } from 'react-bootstrap';

const Loader = ({ loading, children }) => {
  return loading ? (
    <div className="d-flex align-items-center justify-content-center py-4">
      <Spinner animation="border" variant="danger" />
    </div>
  ) : (
    children
  );
};

export default Loader;
