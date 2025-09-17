import React from 'react';
import { Table } from 'react-bootstrap';
import Td from './Td';
import Tr from './Tr';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

const CustomTable = ({ columns, data, pagenation, setPagenation }) => {
  const _languageService = DependenciesInjector.services.languageService;

  const handleOnNextClick = () => {
    if (!setPagenation) return;

    setPagenation(prev => ({
      pages: prev.pages,
      currentPage: ++prev.currentPage,
      perform: true
    }));
  };

  const handleOnPrevClick = () => {
    if (!setPagenation) return;

    setPagenation(prev => ({
      pages: prev.pages,
      currentPage: --prev.currentPage,
      perform: true
    }));
  };

  return (
    <div>
      <Table striped responsive>
        <thead>
          <tr>
            {columns.map((col, index) => (
              <th
                key={index}
                className={
                  col.media ? `d-none d-${col.media}-table-cell px-3` : 'px-3'
                }
              >
                {col.header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {!data || data?.length === 0 ? (
            <Tr>
              <Td colSpan={columns.length} className="text-center py-2">
                {_languageService.resources.noData}
              </Td>
            </Tr>
          ) : (
            data.map((data, index) => (
              <Tr key={index}>
                {columns.map((col, colIndex) => (
                  <Td key={colIndex} {...col.cellProps}>
                    {col.Cell(data)}
                  </Td>
                ))}
              </Tr>
            ))
          )}
        </tbody>
      </Table>

      {pagenation ? (
        <div className="pagenation d-flex align-items-center gap-2 flex-row-reverse px-3">
          <button
            className="btn btn-primary"
            onClick={handleOnNextClick}
            disabled={
              pagenation.pages != null &&
              pagenation.currentPage >= pagenation.pages
            }
          >
            {_languageService.resources.next}
          </button>
          <input
            type="number"
            min={1}
            value={pagenation.currentPage}
            className="form-control"
            readOnly
          />
          <button
            className="btn btn-primary"
            onClick={handleOnPrevClick}
            disabled={pagenation.currentPage <= 1}
          >
            {_languageService.resources.prev}
          </button>
        </div>
      ) : null}
    </div>
  );
};

export default CustomTable;
