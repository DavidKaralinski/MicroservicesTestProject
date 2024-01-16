"use client";

import { Button } from "flowbite-react";
import React, { useEffect } from "react";
import { FieldArray, useForm } from "react-hook-form";
import InputField from "../components/InputField";
import DateTimeInputField from "../components/DateTimeInputField";
import { Auction } from "@/types";

type AuctionFormProps = {
  onSubmit: (data: Auction) => void;
  isProcessing: boolean;
}

export default function AuctionForm(props: AuctionFormProps) {
  const {
    control,
    handleSubmit,
    setFocus,
    formState: { isSubmitting, isValid, errors },
  } = useForm({ mode: "onTouched" });

  useEffect(() => {
    setFocus("make");
  }, [setFocus]);

  const onSubmit = (data: FieldArray) => {
    props.onSubmit(data as Auction);
  };

  return (
    <form className="flex flex-col mt-3" onSubmit={handleSubmit(onSubmit)}>
      <InputField
        label="Make"
        name="make"
        control={control}
        rules={{ required: "Make is required" }}
      />
      <InputField
        label="Model"
        name="model"
        control={control}
        rules={{ required: "Model is required" }}
      />

      <InputField
        label="Color"
        name="color"
        control={control}
        rules={{ required: "Color is required" }}
      />

      <div className="grid grid-cols-2 gap-3">
        <InputField
          label="Year"
          name="year"
          type="number"
          control={control}
          rules={{ required: "Year is required" }}
        />
        <InputField
          label="Mileage"
          name="mileage"
          type="number"
          control={control}
          rules={{ required: "Mileage is required" }}
        />
      </div>

      <div className="grid grid-cols-2 gap-3">
        <InputField
          label="Reserve price"
          name="reservePrice"
          type="number"
          control={control}
        />
        <DateTimeInputField
          label="Auction end date and time"
          name="auctionEnd"
          control={control}
          dateFormat='dd MMMM yyyy hh:mm'
          showTimeSelect
          rules={{ required: "Auction end date is required" }}
        />
      </div>

      <InputField
        label="Image url"
        name="imageUrl"
        control={control}
        rules={{ required: "Image url is required" }}
      />

      <div className="flex justify-between ">
        <Button outline color="gray">
          Cancel
        </Button>
        <Button
          outline
          color="success"
          type="submit"
          disabled={!isValid}
          isProcessing={props.isProcessing}
        >
          Submit
        </Button>
      </div>
    </form>
  );
}
