import FormCard from 'components/shared/FormCard';
import React, { useState, useEffect } from 'react';
import { Form, Button, Card } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { useNavigate } from 'react-router-dom';
import Loader from 'components/shared/Loader';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';

const Create = () => {
  const _languageService = useService(Services.LanguageService);
  const _roleService = useService(Services.RoleService);
  const _accessOfAreaService = useService(Services.AccessOfAreaService);

  // States
  const [formData, setFormData] = useState({
    arabicName: '',
    englishName: '',
    areasOfAccessIds: []
  });
  const [areasOfAccess, setAreasOfAccess] = useState([]);
  const [loading, setLoading] = useState(true);
  const { handleOnChange } = useEvents(setFormData);
  const navigate = useNavigate();

  useEffect(() => {
    loadAreasOfAccess();
  }, []);

  const loadAreasOfAccess = async () => {
    setLoading(true);
    const areas = await _accessOfAreaService.getAllAsync();
    setAreasOfAccess(areas);
    setLoading(false);
  };

  const handleToggleAreaOfAccess = areaId => {
    setFormData(prev => {
      const currentIds = prev.areasOfAccessIds;
      const isSelected = currentIds.includes(areaId);

      return {
        ...prev,
        areasOfAccessIds: isSelected
          ? currentIds.filter(id => id !== areaId)
          : [...currentIds, areaId]
      };
    });
  };

  const handleOnSubmitAsync = async e => {
    e.preventDefault();

    const response = await _roleService.createAsync(formData);
    if (!response.succeeded) return;

    navigate('/roles');
  };

  return (
    <FormCard
      header={_languageService.resources.createRole}
      smallHeader={_languageService.resources.fillFields}
    >
      <Loader loading={loading}>
        <form onSubmit={handleOnSubmitAsync}>
          <Form.Group className="mb-2">
            <Form.Label>
              <span>{_languageService.resources.arabicName}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.arabicName}
              onChange={x =>
                handleOnChange('arabicName', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-4">
            <Form.Label>
              <span>{_languageService.resources.englishName}</span>
              <span className="text-danger fw-bold">*</span>
            </Form.Label>
            <Form.Control
              type="text"
              placeholder={_languageService.resources.englishName}
              onChange={x =>
                handleOnChange('englishName', x.currentTarget.value)
              }
            />
          </Form.Group>

          <Form.Group className="mb-4">
            <Form.Label>
              <span>{_languageService.resources.areasOfAccess}</span>
            </Form.Label>
            <div className="border rounded p-3 bg-light">
              {areasOfAccess.length > 0 ? (
                <div className="row g-3">
                  {areasOfAccess.map((area, index) => {
                    const isSelected = formData.areasOfAccessIds.includes(
                      area.id
                    );
                    return (
                      <div key={index} className="col-md-6">
                        <Card
                          className={`cursor-pointer h-100 ${
                            isSelected
                              ? 'border-primary shadow-sm'
                              : 'border-secondary'
                          }`}
                          onClick={() => handleToggleAreaOfAccess(area.id)}
                          style={{
                            transition: 'all 0.2s'
                          }}
                        >
                          <Card.Body>
                            <Form.Check
                              type="checkbox"
                              checked={isSelected}
                              onChange={() => handleToggleAreaOfAccess(area.id)}
                              onClick={e => e.stopPropagation()}
                              label={
                                <div className="ms-2">
                                  <div className="fw-bold mb-1">
                                    {_languageService.isRTL
                                      ? area.arabicName
                                      : area.englishName}
                                  </div>
                                  <div className="text-muted small">
                                    {_languageService.isRTL
                                      ? area.arabicDescription
                                      : area.englishDescription}
                                  </div>
                                </div>
                              }
                            />
                          </Card.Body>
                        </Card>
                      </div>
                    );
                  })}
                </div>
              ) : (
                <div className="text-muted text-center py-4">
                  {_languageService.resources.noDataAvailable}
                </div>
              )}
            </div>
          </Form.Group>

          <Button
            variant="primary"
            type="submit"
            disabled={
              formData.arabicName.trim() === '' ||
              formData.englishName.trim() === ''
            }
          >
            {_languageService.resources.create}
          </Button>
        </form>
      </Loader>
    </FormCard>
  );
};

export default Create;
