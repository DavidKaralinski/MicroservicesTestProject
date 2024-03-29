"use client";

import { Label, TextInput } from "flowbite-react";
import React from "react";
import { UseControllerProps, useController } from "react-hook-form";

type Props = {
  label?: string;
  type?: string;
  showLabel?: string;
} & UseControllerProps;

export default function InputField(props: Props) {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <div className="mb-3">
      {(props.label && props.showLabel) && (
        <div className="mb-2 block">
          <Label htmlFor={field.name} value={props.label} />
        </div>
      )}
      <TextInput
        {...props}
        {...field}
        placeholder={props.label}
        type={props.type ?? 'text'}
        color={
          fieldState.error ? "failure" : fieldState.isDirty ? "success" : ""
        }
        helperText={fieldState.error?.message as string}
      />
    </div>
  );
}
