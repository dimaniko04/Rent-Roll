import logo from "@assets/icons/logoWhite.svg";
import logoText from "@assets/icons/logoTextWhite.svg";

export const Footer = () => {
  return (
    <div className="w-full bg-primary p-6">
      <div className="flex flex-row items-center gap-x-4">
        <img src={logo} alt="" className="white-svg" />
        <img src={logoText} alt="" className="white-svg" />
      </div>
    </div>
  );
};
