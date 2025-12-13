import React from 'react';
import { Card } from 'react-bootstrap';

const FormCard = ({ header, smallHeader, children }) => {
  return (
    <Card className="rounded">
      <Card.Header>
        <div className="mb-0">
          <h3 className="mb-0">{header}</h3>
          {smallHeader && <small>{smallHeader}</small>}
        </div>
      </Card.Header>
      <Card.Body>{children}</Card.Body>
    </Card>
  );
};

export default FormCard;
