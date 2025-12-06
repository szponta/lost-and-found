import type { Ref } from "react";

type inputType = "text" | "number" | "date";

interface Props {
  id: string;
  labelText: string;
  type?: inputType;
  inputClasses?: string[];
  ref?: Ref<HTMLInputElement>;
  placeholder?: string;
  required?: boolean;
}

const FormGroupInput = ({
  id,
  labelText,
  ref = null,
  type = "text",
  inputClasses = [],
  placeholder = "",
  required = true,
}: Props) => {
  return (
    <div className="form-group">
      <label htmlFor={id}>{labelText}</label>
      <input
        ref={ref}
        className={inputClasses.join(" ")}
        type={type}
        id={id}
        placeholder={placeholder}
        required={required}
      />
    </div>
  );
};

export default FormGroupInput;
