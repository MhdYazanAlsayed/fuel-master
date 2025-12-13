import React from 'react';
import { Breadcrumb } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';

const formatSegment = segment =>
  decodeURIComponent(segment)
    .replace(/-/g, ' ')
    .replace(/\b\w/g, char => char.toUpperCase());

const PathBreadcrumb = () => {
  const { pathname } = useLocation();
  const segments = pathname
    .split('/')
    .filter(Boolean)
    .filter(segment => isNaN(Number(segment)));

  if (segments.length === 0) return null;

  return (
    <div className="px-3 pt-2 mb-2">
      <Breadcrumb>
        <Breadcrumb.Item linkAs={Link} linkProps={{ to: '/' }}>
          <div className="badge bg-primary">Home</div>
        </Breadcrumb.Item>
        {segments.map((segment, index) => {
          const routeTo = `/${segments.slice(0, index + 1).join('/')}`;
          const isLast = index === segments.length - 1;
          const label = formatSegment(segment);

          return isLast ? (
            <Breadcrumb.Item key={routeTo} active>
              <div className="badge bg-primary">{label}</div>
            </Breadcrumb.Item>
          ) : (
            <Breadcrumb.Item
              key={routeTo}
              linkAs={Link}
              linkProps={{ to: routeTo }}
            >
              <div className="badge bg-primary">{label}</div>
            </Breadcrumb.Item>
          );
        })}
      </Breadcrumb>
    </div>
  );
};

export default PathBreadcrumb;
