import React, { createContext, useContext, useState, ReactNode } from 'react';

type AuthScreen = 'login' | 'register' | 'forgot-password' | 'reset-password';

interface AuthContextType {
  isAuthenticated: boolean;
  authScreen: AuthScreen;
  setAuthScreen: (screen: AuthScreen) => void;
  login: (email: string, password: string) => void;
  logout: () => void;
  register: (data: any) => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [authScreen, setAuthScreen] = useState<AuthScreen>('login');

  const login = (email: string, password: string) => {
    // Mock login
    setIsAuthenticated(true);
  };

  const logout = () => {
    setIsAuthenticated(false);
    setAuthScreen('login');
  };

  const register = (data: any) => {
    // Mock register
    setIsAuthenticated(true);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, authScreen, setAuthScreen, login, logout, register }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}
