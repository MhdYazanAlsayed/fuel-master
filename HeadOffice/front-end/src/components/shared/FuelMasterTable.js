import React from 'react';
import { Button, Card } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import Flex from 'components/theme/common/Flex';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const FuelMasterTable = ({
  title,
  buttons,
  columns,
  data,
  pagination,
  setPagination,
  handleOpenSearch,
  handleOpenFilter
}) => {
  const _languageService = useService(Services.LanguageService);

  const handleOnPreviousClick = () => {
    if (pagination.pages <= 1) return;

    setPagination(prev => ({
      ...prev,
      currentPage: --prev.currentPage,
      perform: true
    }));
  };

  const handleOnNextClick = () => {
    if (pagination.currentPage >= pagination.pages) return;

    setPagination(prev => ({
      ...prev,
      currentPage: ++prev.currentPage,
      perform: true
    }));
  };

  return (
    <Card className="mb-3">
      <Card.Header>
        <div className="d-flex align-items-center justify-content-between">
          <h5>{title}</h5>
          <div>{buttons}</div>
        </div>
        <Flex alignItems="center" className="gap-2">
          {handleOpenSearch ? (
            <Button variant="primary" size="sm" onClick={handleOpenSearch}>
              <FontAwesomeIcon icon="search" />
            </Button>
          ) : null}
          {handleOpenFilter ? (
            <Button variant="primary" size="sm">
              <FontAwesomeIcon icon="filter" />
            </Button>
          ) : null}
        </Flex>
      </Card.Header>
      <Card.Body className="p-0">
        {/* Columns */}
        <table className="fs--1 mb-0 table table-sm table-striped">
          <thead className="bg-200 text-900 text-nowrap align-middle">
            <tr>
              {columns.map((col, index) => (
                <th
                  key={index}
                  style={{ cursor: 'pointer' }}
                  {...col.headerProps}
                  role="columnheader"
                >
                  {col.header}
                  {col.isSortable ? <span className="sort"></span> : null}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {!data || data?.length === 0 ? (
              <tr role="row" className="align-middle white-space-nowrap">
                <td colSpan={columns.length} className="text-center py-2">
                  {_languageService.resources.thereIsNoDate}
                </td>
              </tr>
            ) : (
              data.map((data, index) => (
                <tr
                  role="row"
                  className="align-middle white-space-nowrap"
                  key={index}
                >
                  {columns.map((col, colIndex) => (
                    <td role="cell" key={colIndex} {...col.cellProps}>
                      {col.Cell(data)}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </Card.Body>
      <Card.Footer>
        {pagination && (
          <div className="d-flex justify-content-end gap-1">
            <Button
              variant="falcon-default"
              size="sm"
              disabled={pagination.currentPage <= 1}
              onClick={handleOnPreviousClick}
            >
              {_languageService.resources.prev}
            </Button>
            <input
              type="text"
              className="form-control"
              style={{ width: '50px' }}
              value={1}
              readOnly
            />
            <Button
              variant="falcon-default"
              size="sm"
              disabled={pagination.currentPage >= pagination.pages}
              onClick={handleOnNextClick}
            >
              {_languageService.resources.next}
            </Button>
          </div>
        )}
      </Card.Footer>
    </Card>
  );
};

export default FuelMasterTable;
