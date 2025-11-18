const { createSlice } = require('@reduxjs/toolkit');

const citySlice = createSlice({
  name: 'city',
  initialState: [],
  reducers: {
    getCity,
    updateCity
  }
});

function getCity(state, action) {
  return state.find(x => x.data.id === action.payload.id)?.data;
}

function updateCity(state, action) {
  const index = state.find(x => x.data.id == action.payload.id);

  state[index] = { ...state[index], data: x.action.payload };
}
