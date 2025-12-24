import FormCard from 'components/shared/FormCard';
import React, { useEffect, useState } from 'react';
import { Form, Card } from 'react-bootstrap';
import { Navigate, useParams } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const Details = () => {
  const _languageService = useService(Services.LanguageService);
  const _roleService = useService(Services.RoleService);

  const [roleDetails, setRoleDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const { id } = useParams();

  if (!id) return <Navigate to={'/errors/404'} />;

  useEffect(() => {
    const loadRoleDetails = async () => {
      setLoading(true);
      const details = await _roleService.detailsAsync(id);
      setRoleDetails(details);
      setLoading(false);
    };

    loadRoleDetails();
  }, [id]);

  return (
    <FormCard
      header={_languageService.resources.details}
      smallHeader={_languageService.resources.roleDetails}
    >
      <Loader loading={loading}>
        {roleDetails && (
          <div className="row">
            <div className="col-md-6">
              <Form.Group className="mb-3">
                <Form.Label>{_languageService.resources.arabicName}</Form.Label>
                <Form.Control
                  type="text"
                  value={roleDetails?.arabicName ?? ''}
                  readOnly
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>
                  {_languageService.resources.englishName}
                </Form.Label>
                <Form.Control
                  type="text"
                  value={roleDetails?.englishName ?? ''}
                  readOnly
                />
              </Form.Group>
            </div>
            <div className="col-md-6">
              <Form.Group className="mb-3">
                <Form.Label>
                  {_languageService.resources.areasOfAccess}
                </Form.Label>
                <div
                  className="border rounded p-3 bg-light"
                  style={{ maxHeight: '400px', overflowY: 'auto' }}
                >
                  {roleDetails?.areasOfAccess?.length > 0 ? (
                    <div className="row g-3">
                      {roleDetails.areasOfAccess.map((area, index) => (
                        <div key={index} className="col-md-6">
                          <Card className="h-100 shadow-sm">
                            <Card.Body>
                              <div className="fw-bold mb-2">
                                {_languageService.isRTL
                                  ? area.arabicName
                                  : area.englishName}
                              </div>
                              <div className="text-muted small">
                                {_languageService.isRTL
                                  ? area.arabicDescription
                                  : area.englishDescription}
                              </div>
                            </Card.Body>
                          </Card>
                        </div>
                      ))}
                    </div>
                  ) : (
                    <div className="text-muted text-center py-4">
                      {_languageService.resources.noDataAvailable}
                    </div>
                  )}
                </div>
              </Form.Group>
            </div>
          </div>
        )}
      </Loader>
    </FormCard>
  );
};

export default Details;

