'use client'

import Countdown, { CountdownRendererFn, zeroPad } from "react-countdown";

type CountdownTimerProps = {
  endDate: string;
};

export const CountdownTimer = (props: CountdownTimerProps) => {
    const renderer: CountdownRendererFn = ({
        days,
        hours,
        minutes,
        seconds,
        completed,
      }) => {
        return (
          <div
            className={`border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center
          ${
            completed
              ? "bg-red-600"
              : days === 0 && hours < 10
              ? "bg-amber-600"
              : "bg-green-600"
          }`}
          >
            {completed ? (
              <span>Auction finished</span>
            ) : (
              <span>
                {zeroPad(days)}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
              </span>
            )}
          </div>
        );
      };

      
  return <Countdown renderer={renderer} date={props.endDate} />;
};
