"use client";

import React from "react";
import { UseControllerProps, useController } from "react-hook-form";
import 'react-datepicker/dist/react-datepicker.css';
import DatePicker, { ReactDatePickerProps } from "react-datepicker";

type Props = {
  label?: string;
  type?: string;
} & UseControllerProps & Partial<ReactDatePickerProps>;

export default function DateTimeInputField(props: Props) {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <div className='block mb-3'>
      <DatePicker
        {...props}
        {...field}
        onChange={(value) => field.onChange(value)}
        selected={field.value}
        placeholderText={props.label}
        className={`
          rounded-lg w-[100%] flex flex-col
          ${fieldState.error ?
              'bg-red-50 border-red-500 text-red-800' :
              !fieldState.invalid && fieldState.isDirty ?
                'bg-green-50 border-green-500 text-green-800' : ''
          }
        `}
      />
      {fieldState.error && (
        <div className='text-red-500 text-sm mt-2'>{fieldState.error.message}</div>
      )}
    </div>
  );
}
