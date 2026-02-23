export const useDate = () => {
  const formatDate = (dateStr: string, includeTime = true) => {
    const date = new Date(dateStr);
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      ...(includeTime ? { hour: '2-digit', minute: '2-digit' } : {}),
    };
    return date.toLocaleString(undefined, options);
  };

  return { formatDate };
};
