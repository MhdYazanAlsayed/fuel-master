import React, { useState, useEffect } from 'react';
import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import ModalTop from 'components/shared/ModalTop';
import { Button, Form, Modal } from 'react-bootstrap';
import { useEvents } from 'hooks/useEvents';
import { toast } from 'react-toastify';
import { Permissions } from 'app/core/enums/Permissions';
import { Navigate } from 'react-router-dom';

const _languageService = DependenciesInjector.services.languageService;
const _employeeService = DependenciesInjector.services.employeeSerivce;
const _roleManager = DependenciesInjector.services.roleManager;

const EditPasswordModal = ({ open, setOpen, employeeId }) => {
  if (!_roleManager.check(Permissions.EmployeesEdit))
    return <Navigate to="/errors/404" />;

  const [formData, setFormData] = useState({
    password: '',
    confirmPassword: ''
  });
  const { handleOnChange } = useEvents(setFormData);

  useEffect(() => {
    setFormData({
      password: '',
      confirmPassword: ''
    });
  }, [open]);

  const handleOnSubmitAsync = async e => {
    e.preventDefault();
    if (!employeeId) return;

    if (formData.password.trim() !== formData.confirmPassword.trim()) {
      toast.error(_languageService.resources.passwordsDontMatch);
      return;
    }
    const response = await _employeeService.editPasswordAsync(
      employeeId,
      formData
    );
    if (!response.succeeded) return;

    setOpen(false);
  };

  return (
    <ModalTop
      open={open}
      setOpen={setOpen}
      title={_languageService.resources.editPassword}
    >
      <form onSubmit={handleOnSubmitAsync}>
        <Modal.Body>
          <Form.Group className="mb-2">
            <Form.Label>
              {_languageService.resources.password}
              <span className="text-danger">*</span>
            </Form.Label>

            <Form.Control
              type="password"
              value={formData.password}
              minLength={6}
              maxLength={100}
              required
              onChange={x => handleOnChange('password', x.currentTarget.value)}
            />
          </Form.Group>

          <Form.Group className="mb-2">
            <Form.Label>
              {_languageService.resources.confirmPassword}
              <span className="text-danger">*</span>
            </Form.Label>

            <Form.Control
              type="password"
              value={formData.confirmPassword}
              minLength={6}
              maxLength={100}
              required
              onChange={x =>
                handleOnChange('confirmPassword', x.currentTarget.value)
              }
            />
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <div className="d-flex align-items-center gap-2">
            <button
              className="btn btn-primary"
              type="submit"
              disabled={
                formData.password.trim() === '' ||
                formData.confirmPassword.trim() === '' ||
                formData.password.trim() !== formData.confirmPassword.trim() ||
                formData.password.trim().length < 6
              }
            >
              {_languageService.resources.edit}
            </button>

            <Button
              variant="secondary"
              onClick={e => {
                e.preventDefault();
                setOpen(false);
              }}
            >
              {_languageService.resources.cancle}
            </Button>
          </div>
        </Modal.Footer>
      </form>
    </ModalTop>
  );
};

export default EditPasswordModal;
