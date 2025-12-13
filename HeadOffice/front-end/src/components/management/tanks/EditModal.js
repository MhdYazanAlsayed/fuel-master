import React, { useEffect, useState } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import { Form, Button, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import ModalCenter from 'components/shared/ModalCenter';
import { toast } from 'react-toastify';

const _languageService = DependenciesInjector.services.languageService;
const _tankService = DependenciesInjector.services.tankService;

const EditModal = ({ open, setOpen, tank, handleUpdateTank }) => {
  const [formData, setFormData] = useState({
    capacity: 0,
    maxLimit: 0,
    minLimit: 0,
    currentLevel: 0,
    currentVolume: 0,
    hasSensor: false
  });
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    if (!open || !tank) return;

    setFormData({
      capacity: tank.capacity ?? 0,
      maxLimit: tank.maxLimit ?? 0,
      minLimit: tank.minLimit ?? 0,
      currentLevel: tank.currentLevel ?? 0,
      currentVolume: tank.currentVolume ?? 0,
      hasSensor: tank.hasSensor ?? false
    });
  }, [open, tank]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!tank) return;

    if (
      formData.capacity <= 0 ||
      formData.maxLimit <= 0 ||
      formData.minLimit <= 0
    ) {
      toast.error(_languageService.resources.valuesNotValid);
      return;
    }

    if (formData.maxLimit >= formData.capacity) {
      toast.error(_languageService.resources.maxLimitMustBeLessThanCapacity);
      return;
    }

    if (formData.minLimit >= formData.maxLimit) {
      toast.error(_languageService.resources.minLimitMustBeLessThanMaxLimit);
      return;
    }

    if (formData.currentVolume > formData.maxLimit) {
      toast.error(
        _languageService.resources.currentVolumeMustBeLessThanMaxLimit
      );
      return;
    }

    if (formData.currentVolume < formData.minLimit) {
      toast.error(
        _languageService.resources.currentVolumeMustBeGreaterThanMinLimit
      );
      return;
    }

    const updateData = {
      capacity: formData.capacity,
      maxLimit: formData.maxLimit,
      minLimit: formData.minLimit,
      currentLevel: formData.currentLevel,
      currentVolume: formData.currentVolume,
      hasSensor: formData.hasSensor
    };

    const response = await _tankService.editAsync(tank.id, updateData);
    if (!response.succeeded) return;

    handleUpdateTank(response.entity);
    setOpen(false);
  };

  const isDisabled =
    !tank ||
    formData.capacity <= 0 ||
    formData.maxLimit <= 0 ||
    formData.minLimit <= 0 ||
    formData.currentLevel < 0 ||
    formData.currentVolume < 0 ||
    formData.maxLimit >= formData.capacity ||
    formData.minLimit >= formData.maxLimit ||
    formData.currentVolume > formData.maxLimit ||
    formData.currentVolume < formData.minLimit ||
    (formData.capacity === (tank?.capacity ?? 0) &&
      formData.maxLimit === (tank?.maxLimit ?? 0) &&
      formData.minLimit === (tank?.minLimit ?? 0) &&
      formData.currentLevel === (tank?.currentLevel ?? 0) &&
      formData.currentVolume === (tank?.currentVolume ?? 0) &&
      formData.hasSensor === (tank?.hasSensor ?? false));

  return (
    <ModalCenter
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editTank}
      size={'lg'}
    >
      <Modal.Body>
        <form onSubmit={handleOnSubmitAsync}>
          <div className="row">
            <div className="col-md-6">
              <Form.Group className="mb-2">
                <Form.Label>
                  <span>{_languageService.resources.capacity}</span>
                  <span className="text-danger fw-bold">*</span>
                </Form.Label>
                <Form.Control
                  type="number"
                  placeholder={_languageService.resources.capacity}
                  value={formData.capacity}
                  onChange={x =>
                    handleOnChange(
                      'capacity',
                      parseFloat(x.currentTarget.value)
                    )
                  }
                />
              </Form.Group>

              <Form.Group className="mb-2">
                <Form.Label>
                  <span>{_languageService.resources.maxLimit}</span>
                  <span className="text-danger fw-bold">*</span>
                </Form.Label>
                <Form.Control
                  type="number"
                  placeholder={_languageService.resources.maxLimit}
                  value={formData.maxLimit}
                  onChange={x =>
                    handleOnChange(
                      'maxLimit',
                      parseFloat(x.currentTarget.value)
                    )
                  }
                />
              </Form.Group>

              <Form.Group className="mb-2">
                <Form.Label>
                  <span>{_languageService.resources.minLimit}</span>
                  <span className="text-danger fw-bold">*</span>
                </Form.Label>
                <Form.Control
                  type="number"
                  placeholder={_languageService.resources.minLimit}
                  value={formData.minLimit}
                  onChange={x =>
                    handleOnChange(
                      'minLimit',
                      parseFloat(x.currentTarget.value)
                    )
                  }
                />
              </Form.Group>
            </div>
            <div className="col-md-6">
              <Form.Group className="mb-2">
                <Form.Label>
                  <span>{_languageService.resources.currentLevel}</span>
                  <span className="text-danger fw-bold">*</span>
                </Form.Label>
                <Form.Control
                  type="number"
                  placeholder={_languageService.resources.currentLevel}
                  value={formData.currentLevel}
                  onChange={x =>
                    handleOnChange(
                      'currentLevel',
                      parseFloat(x.currentTarget.value)
                    )
                  }
                />
              </Form.Group>

              <Form.Group className="mb-2">
                <Form.Label>
                  <span>{_languageService.resources.currentVolume}</span>
                  <span className="text-danger fw-bold">*</span>
                </Form.Label>
                <Form.Control
                  type="number"
                  placeholder={_languageService.resources.currentVolume}
                  value={formData.currentVolume}
                  onChange={x =>
                    handleOnChange(
                      'currentVolume',
                      parseFloat(x.currentTarget.value)
                    )
                  }
                />
              </Form.Group>

              <Form.Group className="mb-4">
                <Form.Check
                  type="checkbox"
                  label={_languageService.resources.hasSensor}
                  checked={formData.hasSensor}
                  onChange={x =>
                    handleOnChange('hasSensor', x.currentTarget.checked)
                  }
                />
              </Form.Group>
            </div>
          </div>

          <Button variant="primary" type="submit" disabled={isDisabled}>
            {_languageService.resources.edit}
          </Button>
        </form>
      </Modal.Body>
    </ModalCenter>
  );
};

export default EditModal;
