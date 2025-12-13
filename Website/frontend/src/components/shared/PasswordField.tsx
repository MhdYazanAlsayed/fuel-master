import React, { useState } from "react";

interface PasswordFieldProps {
  id: string;
  name?: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder?: string;
  required?: boolean;
  className?: string;
  label?: string | React.ReactNode;
  showLabel?: boolean;
  error?: string | null;
  wrapperClassName?: string;
}

export function PasswordField({
  id,
  name,
  value,
  onChange,
  placeholder = "Enter your password",
  required = false,
  className = "form-input",
  label,
  showLabel = true,
  error,
  wrapperClassName = "form-group",
}: PasswordFieldProps) {
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div className={wrapperClassName}>
      {showLabel && label && (
        <label htmlFor={id}>
          {typeof label === "string" ? (
            <>
              {label} {required && <span className="required">*</span>}
            </>
          ) : (
            label
          )}
        </label>
      )}
      <div className="form-input-wrapper">
        <svg
          className="form-input-icon"
          width="18"
          height="18"
          viewBox="0 0 18 18"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M13.5 7.5V4.5C13.5 2.84315 12.1569 1.5 10.5 1.5H7.5C5.84315 1.5 4.5 2.84315 4.5 4.5V7.5"
            stroke="currentColor"
            strokeWidth="1.5"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
          <rect
            x="3"
            y="7.5"
            width="12"
            height="9"
            rx="2"
            stroke="currentColor"
            strokeWidth="1.5"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
          <path
            d="M9 12V12.75"
            stroke="currentColor"
            strokeWidth="1.5"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
        </svg>
        <input
          type={showPassword ? "text" : "password"}
          id={id}
          name={name || id}
          className={className}
          placeholder={placeholder}
          value={value}
          onChange={onChange}
          style={{ paddingRight: "3rem" }}
          required={required}
        />
        <div
          style={{
            position: "absolute",
            right: "1.5px",
            top: "1.5px",
            bottom: "1.5px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            padding: "0 0.75rem",
            borderTopRightRadius: "10px",
            borderBottomRightRadius: "10px",
            background: "var(--white)",
            cursor: "pointer",
            transition: "all 0.2s",
          }}
          onClick={() => setShowPassword(!showPassword)}
          onMouseEnter={(e) => {
            e.currentTarget.style.borderLeftColor = "var(--primary-light)";
          }}
          onMouseLeave={(e) => {
            e.currentTarget.style.borderLeftColor = "var(--border-color)";
          }}
          aria-label={showPassword ? "Hide password" : "Show password"}
          role="button"
          tabIndex={0}
          onKeyDown={(e) => {
            if (e.key === "Enter" || e.key === " ") {
              e.preventDefault();
              setShowPassword(!showPassword);
            }
          }}
        >
          {showPassword ? (
            <svg
              width="18"
              height="18"
              viewBox="0 0 18 18"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
              style={{ color: "var(--medium-gray)" }}
            >
              <path
                d="M1.5 9C1.5 9 4.5 4.5 9 4.5C13.5 4.5 16.5 9 16.5 9C16.5 9 13.5 13.5 9 13.5C4.5 13.5 1.5 9 1.5 9Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
              <path
                d="M9 11.25C10.2426 11.25 11.25 10.2426 11.25 9C11.25 7.75736 10.2426 6.75 9 6.75C7.75736 6.75 6.75 7.75736 6.75 9C6.75 10.2426 7.75736 11.25 9 11.25Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
              <path
                d="M1.5 1.5L16.5 16.5"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
          ) : (
            <svg
              width="18"
              height="18"
              viewBox="0 0 18 18"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
              style={{ color: "var(--medium-gray)" }}
            >
              <path
                d="M1.5 9C1.5 9 4.5 4.5 9 4.5C13.5 4.5 16.5 9 16.5 9C16.5 9 13.5 13.5 9 13.5C4.5 13.5 1.5 9 1.5 9Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
              <path
                d="M9 11.25C10.2426 11.25 11.25 10.2426 11.25 9C11.25 7.75736 10.2426 6.75 9 6.75C7.75736 6.75 6.75 7.75736 6.75 9C6.75 10.2426 7.75736 11.25 9 11.25Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
          )}
        </div>
      </div>
      {error && (
        <div
          className="form-error"
          style={{
            color: "#ef4444",
            fontSize: "14px",
            marginTop: "4px",
          }}
        >
          {error}
        </div>
      )}
    </div>
  );
}

