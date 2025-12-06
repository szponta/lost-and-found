import type { Ref } from "react";

type inputType = "text" | "number" | "date";

interface Props {
  key?: string;
  id?: string;
  labelText: string;
  type?: inputType;
  ref?: Ref<HTMLInputElement>;
  placeholder?: string;
  defaultValue?: string;
  required?: boolean;
}

const FormGroupInput = ({
  key,
  id,
  labelText,
  ref = null,
  type = "text",
  placeholder = "",
  required = false,
  defaultValue,
}: Props) => {
  return (
    <div className="form-group">
      <label>{labelText}</label>
      <input
        key={key}
        id={id}
        ref={ref}
        type={type}
        placeholder={placeholder}
        required={required}
        defaultValue={defaultValue}
      />
    </div>
  );
};

export default FormGroupInput;
