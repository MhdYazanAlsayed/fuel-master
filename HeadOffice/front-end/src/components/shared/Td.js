import React from 'react';

const Td = ({ media, colSpan, className, children }) => {
  return (
    <td
      className={
        media
          ? `d-none d-${media}-table-cell px-3${
              className ? ` ${className}` : ''
            }`
          : `px-3${className ? ` ${className}` : ''}`
      }
      colSpan={colSpan}
    >
      {children}
    </td>
  );
};

export default Td;
