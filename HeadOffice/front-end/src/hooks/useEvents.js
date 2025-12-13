export const useEvents = setFormData => {
  function handleOnChange(key, value) {
    setFormData(prev => ({
      ...prev,
      [key]: value
    }));
  }

  return { handleOnChange };
};
